using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Threading;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using AutoMapper;

using ABC.NetCore.Models;
using ABC.NetCore.Infrastructure;

using ABC.NetCore.ProblemSolving.Models;
using ABC.NetCore.ProblemSolving.Services;

namespace ABC.NetCore.ProblemSolving.Controllers
{
    [Route("/api/[controller]")]
    public class CustomersController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly PagingOptions _defaultPagingOptions;

        public CustomersController(
            ICustomerService customerService,
            IOptions<PagingOptions> defaultPagingOptions
            )
        {
            _customerService = customerService;
            _defaultPagingOptions = defaultPagingOptions.Value;
        }

        [HttpGet(Name = nameof(GetCustomersAsync))]
        [ValidateAPIModelState]
        // [ResponseCache(CacheProfileName = "ServerResponseCacheProfile")] // Cache response server side
        public async Task<IActionResult> GetCustomersAsync(
            [FromQuery] PagingOptions pagingOptions,
            [FromQuery] SearchOptions<Customer, CustomerEntity> searchOptions,
            [FromQuery] SortOptions<Customer, CustomerEntity> sortOptions,
            CancellationToken ct)
        {
            pagingOptions.Offset = pagingOptions.Offset ?? _defaultPagingOptions.Offset;
            pagingOptions.Limit = pagingOptions.Limit ?? _defaultPagingOptions.Limit;

            var pagedResult = await _customerService.GetCustomersAsync(pagingOptions, searchOptions, sortOptions, ct);

            var collections = PagedCollection<Customer>.Create<CustomerResponse>(
                Link.ToCollection(nameof(GetCustomersAsync)), pagedResult.Items.ToArray(), pagedResult.Size, pagingOptions);

            // TODO:: HETEOS REST Approach
            // Bellow link can be used to attach helper form object to each object for HETEOS approach with REST API
            // Commented out as it adds too much complexity with every REST API Response.
            // collections.CustomerQuery = FormMetadata.FromResource<Customer>(Link.ToForm(nameof(GetCustomersAsync), null, Link.GetMethod, Form.QueryRelation));

            return Ok(collections);
        }

        [HttpGet("{Id}", Name = nameof(GetCustomerByIdAsync))]
        public async Task<IActionResult> GetCustomerByIdAsync(Guid Id, CancellationToken ct)
        {
            var customer = await _customerService.GetCustomerByIdAsync(Id, ct);
            if (customer == null) return NotFound();

            return Ok(customer);
        }

        //TODO: Authorization 
        [HttpPost(Name = nameof(CreateCustomerAsync))]
        [ValidateAPIModelState]
        public async Task<IActionResult> CreateCustomerAsync(
            [FromBody] CustomerRequest customerForm,
            CancellationToken ct)
        {
            var customer = await _customerService.GetCustomerByNameAsync(customerForm.Name, ct);
            if (customer != null) return StatusCode((int)HttpStatusCode.Conflict);

            Customer customerObj = Mapper.Map<Customer>(customerForm);
            var customerId = await _customerService.CreateCustomerAsync(customerObj, ct);

            return Created(Url.Link(nameof(GetCustomerByIdAsync), new { Id = customerId }), null);
        }

        //TODO: Authorization
        [HttpPut("{Id}", Name = nameof(UpdateCustomerByIdAsync))]
        [ValidateAPIModelState]
        public async Task<IActionResult> UpdateCustomerByIdAsync(
            Guid Id, [FromBody] CustomerRequest customerForm, CancellationToken ct)
        {
            Customer customerObj = Mapper.Map<Customer>(customerForm);
            var updatedCustomer = await _customerService.UpdateCustomerAsync(Id, customerObj, ct);

            if (updatedCustomer == null)
            {
                return BadRequest("Requested customer doesn't exists");
            }
            else
            {
                return Ok(updatedCustomer);
            }
        }

        //TODO: Authorization 
        [HttpDelete("{Id}", Name = nameof(DeleteCustomerByIdAsync))]
        public async Task<IActionResult> DeleteCustomerByIdAsync(Guid Id, CancellationToken ct)
        {
            await _customerService.DeleteCustomerByIDAsync(Id, ct);
            return NoContent();
        }
    }
}
