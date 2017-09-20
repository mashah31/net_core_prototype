using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class SAPController: Controller
    {
        private readonly ISAPService _sapService;
        private readonly PagingOptions _defaultPagingOption;

        public SAPController(ISAPService sapService, IOptions<PagingOptions> defaultPagingOptions)
        {
            _sapService = sapService;
            _defaultPagingOption = defaultPagingOptions.Value;
        }

        [HttpGet("employees", Name = nameof(GetSAPEmployeesAsync))]
        [ValidateAPIModelState]
        [ResponseCache(CacheProfileName = "ServerResponseCacheProfile")] // Cache response server side
        public async Task<IActionResult> GetSAPEmployeesAsync(
            [FromQuery] PagingOptions pagingOptions,
            [FromQuery] SearchOptions<SAPEmployee, SAPEmployeeEntity> searchOptions,
            [FromQuery] SortOptions<SAPEmployee, SAPEmployeeEntity> sortOptions,
            CancellationToken ct)
        {
            pagingOptions.Offset = pagingOptions.Offset ?? _defaultPagingOption.Offset;
            pagingOptions.Limit = pagingOptions.Limit ?? _defaultPagingOption.Limit;

            var pagedResult = await _sapService.GetSAPEmployeesAsync(pagingOptions, searchOptions, sortOptions, ct);

            var collections = PagedCollection<SAPEmployee>.Create<SAPEmployeeResponse>(
                Link.ToCollection(nameof(GetSAPEmployeesAsync)), pagedResult.Items.ToArray(), pagedResult.Size, pagingOptions);

            return Ok(collections);
        }

        [HttpGet("parts", Name = nameof(GetSAPPartsAsync))]
        [ValidateAPIModelState]
        [ResponseCache(CacheProfileName = "ServerResponseCacheProfile")] // Cache response server side
        public async Task<IActionResult> GetSAPPartsAsync(
            [FromQuery] PagingOptions pagingOptions,
            [FromQuery] SearchOptions<SAPPart, SAPPartEntity> searchOptions,
            [FromQuery] SortOptions<SAPPart, SAPPartEntity> sortOptions,
            CancellationToken ct)
        {
            pagingOptions.Offset = pagingOptions.Offset ?? _defaultPagingOption.Offset;
            pagingOptions.Limit = pagingOptions.Limit ?? _defaultPagingOption.Limit;

            var pagedResult = await _sapService.GetSAPPartsAsync(pagingOptions, searchOptions, sortOptions, ct);

            var collections = PagedCollection<SAPPart>.Create<SAPPartResponse>(
                Link.ToCollection(nameof(GetSAPPartsAsync)), pagedResult.Items.ToArray(), pagedResult.Size, pagingOptions);

            return Ok(collections);
        }
    }
}
