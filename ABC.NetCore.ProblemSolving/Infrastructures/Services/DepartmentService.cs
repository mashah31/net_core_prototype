using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

using ABC.NetCore.Models;
using ABC.NetCore.ProblemSolving.Models;
using ABC.NetCore.ProblemSolving.Infrastructures;

namespace ABC.NetCore.ProblemSolving.Services
{
    public interface IDepartmentService
    {
        Task<Department> GetDepartmentByTagAsync(string tag, CancellationToken ct);

        Task<PagedResult<Department>> GetDepartmentsAsync(
            PagingOptions pagingOptions,
            SearchOptions<Department, DepartmentEntity> searchOptions,
            SortOptions<Department, DepartmentEntity> sortOptions,
            CancellationToken ct);

        Task<Guid> CreateDepartmentAsync(
            Department departmentForm, CancellationToken ct);

        Task DeleteDepartmentByIDAsync(Guid Id, CancellationToken ct);
    }

    public class DepartmentService: IDepartmentService
    {
        private readonly ProblemSolvingDBContext _dbContext;

        public DepartmentService(ProblemSolvingDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Department> GetDepartmentByTagAsync(string tag, CancellationToken ct)
        {
            DepartmentEntity entity = await _dbContext.Departments.SingleOrDefaultAsync(d => d.Tag.Equals(tag, StringComparison.OrdinalIgnoreCase), ct);
            if (entity == null) return null;

            return Mapper.Map<Department>(entity);
        }

        public async Task<PagedResult<Department>> GetDepartmentsAsync(
            PagingOptions pagingOptions,
            SearchOptions<Department, DepartmentEntity> searchOptions,
            SortOptions<Department, DepartmentEntity> sortOptions,
            CancellationToken ct)
        {
            IQueryable<DepartmentEntity> query = _dbContext.Departments;

            query = searchOptions.Apply(query);
            query = sortOptions.Apply(query);

            var size = await query.CountAsync(ct);

            var items = await query
                .Skip(pagingOptions.Offset.Value)
                .Take(pagingOptions.Limit.Value)
                .ProjectTo<Department>().ToArrayAsync(ct);

            return new PagedResult<Department>
            {
                Items = items,
                Size = size
            };
        }

        public async Task<Guid> CreateDepartmentAsync(Department departmentForm, CancellationToken ct)
        {
            var id = Guid.NewGuid();

            DepartmentEntity entity = Mapper.Map<DepartmentEntity>(departmentForm);
            entity.Id = id;

            var newObj = _dbContext.Departments.Add(entity);
            var created = await _dbContext.SaveChangesAsync(ct);
            if (created < 1) throw new InvalidOperationException("Could not create the department");

            return id;
        }

        public async Task DeleteDepartmentByIDAsync(Guid Id, CancellationToken ct)
        {
            var objToRemove = await _dbContext.Departments.SingleOrDefaultAsync(c => c.Id == Id, ct);
            if (objToRemove == null) return;

            _dbContext.Departments.Remove(objToRemove);
            var deleted = await _dbContext.SaveChangesAsync(ct);
            if (deleted < 1) throw new InvalidOperationException("Could not delete the object");
        }
    }
}
