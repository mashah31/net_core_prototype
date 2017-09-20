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
    public class ManagementController : Controller
    {
        private readonly IManagementService _managementService;
        private readonly PagingOptions _defaultPagingOptions;

        public ManagementController(
            IManagementService managementService,
            IOptions<PagingOptions> defaultPagingOptions
            )
        {
            _managementService = managementService;
            _defaultPagingOptions = defaultPagingOptions.Value;
        }

        [HttpGet("/complaintcodes")]
        [ValidateAPIModelState]
        [ResponseCache(CacheProfileName = "ServerResponseCacheProfile")]
        public async Task<IActionResult> GetComplaintCodesAsync(
            [FromQuery] PagingOptions pagingOptions,
            [FromQuery] SearchOptions<ComplaintCode, ComplaintCodeEntity> searchOptions,
            [FromQuery] SortOptions<ComplaintCode, ComplaintCodeEntity> sortOptions,
            CancellationToken ct)
        {
            pagingOptions.Offset = pagingOptions.Offset ?? _defaultPagingOptions.Offset;
            pagingOptions.Limit = pagingOptions.Limit ?? _defaultPagingOptions.Limit;

            var pagedResult = await _managementService.GetComplaintCodesAsync(pagingOptions, searchOptions, sortOptions, ct);

            var collections = PagedCollection<ComplaintCode>.Create<ComplaintCodeResponce>(
                Link.ToCollection(nameof(GetComplaintCodesAsync)), pagedResult.Items.ToArray(), pagedResult.Size, pagingOptions);
            
            return Ok(collections);
        }
    }
}
