using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

using ABC.NetCore.Models;
using ABC.NetCore.ProblemSolving.Models;
using ABC.NetCore.ProblemSolving.Infrastructures;

namespace ABC.NetCore.ProblemSolving.Services
{
    public interface ISAPService
    {
        Task<PagedResult<SAPPart>> GetSAPPartsAsync(
            PagingOptions pagingOptions,
            SearchOptions<SAPPart, SAPPartEntity> searchOptions,
            SortOptions<SAPPart, SAPPartEntity> sortOptions,
            CancellationToken ct);

        Task<PagedResult<SAPEmployee>> GetSAPEmployeesAsync(
            PagingOptions pagingOptions,
            SearchOptions<SAPEmployee, SAPEmployeeEntity> searchOptions,
            SortOptions<SAPEmployee, SAPEmployeeEntity> sortOptions,
            CancellationToken ct);
    }

    public class SAPService:ISAPService
    {
        private readonly CentralDataDBContext _dbContext;

        public SAPService(CentralDataDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedResult<SAPPart>> GetSAPPartsAsync(
            PagingOptions pagingOptions, 
            SearchOptions<SAPPart, SAPPartEntity> searchOptions, 
            SortOptions<SAPPart, SAPPartEntity> sortOptions, 
            CancellationToken ct)
        {
            IQueryable<SAPPartEntity> query = _dbContext.SAPParts;

            query = searchOptions.Apply(query);
            query = sortOptions.Apply(query);

            var size = await query.CountAsync(ct);

            var items = await query
                .Skip(pagingOptions.Offset.Value)
                .Take(pagingOptions.Limit.Value)
                .ProjectTo<SAPPart>().ToArrayAsync(ct);

            return new PagedResult<SAPPart>
            {
                Items = items,
                Size = size
            };
        }

        public async Task<PagedResult<SAPEmployee>> GetSAPEmployeesAsync(
            PagingOptions pagingOptions, 
            SearchOptions<SAPEmployee, SAPEmployeeEntity> searchOptions, 
            SortOptions<SAPEmployee, SAPEmployeeEntity> sortOptions, 
            CancellationToken ct)
        {
            IQueryable<SAPEmployeeEntity> query = _dbContext.SAPEmployees
                .Where(emp => emp.UserName != null && emp.UserName.Trim() != "");

            query = searchOptions.Apply(query);
            query = sortOptions.Apply(query);

            var size = await query.CountAsync(ct);

            var items = await query
                .Skip(pagingOptions.Offset.Value)
                .Take(pagingOptions.Limit.Value)
                .ProjectTo<SAPEmployee>().ToArrayAsync(ct);

            return new PagedResult<SAPEmployee>
            {
                Items = items,
                Size = size
            };
        }
    }
}
