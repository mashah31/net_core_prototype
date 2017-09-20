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
    public interface IProblemSolvingTypeService
    {
        Task<ProblemSolvingType> GetProblemSolvingTypeByTagAsync(string tag, CancellationToken ct);

        Task<PagedResult<ProblemSolvingType>> GetProblemSolvingTypeAsync(
            PagingOptions pagingOptions,
            SearchOptions<ProblemSolvingType, ProblemSolvingTypeEntity> searchOptions,
            SortOptions<ProblemSolvingType, ProblemSolvingTypeEntity> sortOptions,
            CancellationToken ct);

        Task<Guid> CreateProblemSolvingTypeAsync(
            ProblemSolvingType problemSolvingTypeForm, CancellationToken ct);

        Task<ProblemSolvingType> UpdateProblemSolvingTypeAsync(
            Guid Id, ProblemSolvingType problemSolvingTypeForm, CancellationToken ct);

        Task DeleteProblemSolvingTypeByIDAsync(Guid Id, CancellationToken ct);
    }

    public class ProblemSolvingTypeService: IProblemSolvingTypeService
    {
        private readonly ProblemSolvingDBContext _dbContext;

        public ProblemSolvingTypeService(ProblemSolvingDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Guid> CreateProblemSolvingTypeAsync(ProblemSolvingType problemSolvingTypeForm, CancellationToken ct)
        {
            var id = Guid.NewGuid();

            ProblemSolvingTypeEntity entity = Mapper.Map<ProblemSolvingTypeEntity>(problemSolvingTypeForm);
            entity.Id = id;

            var newObj = _dbContext.ProblemSolvingTypes.Add(entity);
            var created = await _dbContext.SaveChangesAsync(ct);
            if (created < 1) throw new InvalidOperationException("Could not create the requested object");

            return id;
        }

        public async Task DeleteProblemSolvingTypeByIDAsync(Guid Id, CancellationToken ct)
        {
            var objToRemove = await _dbContext.ProblemSolvingTypes.SingleOrDefaultAsync(c => c.Id == Id, ct);
            if (objToRemove == null) return;

            _dbContext.ProblemSolvingTypes.Remove(objToRemove);
            var deleted = await _dbContext.SaveChangesAsync(ct);
            if (deleted < 1) throw new InvalidOperationException("Could not delete the object");
        }

        public async Task<ProblemSolvingType> GetProblemSolvingTypeByTagAsync(string tag, CancellationToken ct)
        {
            ProblemSolvingTypeEntity entity = await _dbContext.ProblemSolvingTypes.SingleOrDefaultAsync(i => i.Tag.Equals(tag, StringComparison.OrdinalIgnoreCase), ct);
            if (entity == null) return null;

            return Mapper.Map<ProblemSolvingType>(entity);
        }

        public async Task<PagedResult<ProblemSolvingType>> GetProblemSolvingTypeAsync(
            PagingOptions pagingOptions,
            SearchOptions<ProblemSolvingType, ProblemSolvingTypeEntity> searchOptions,
            SortOptions<ProblemSolvingType, ProblemSolvingTypeEntity> sortOptions,
            CancellationToken ct)
        {
            IQueryable<ProblemSolvingTypeEntity> query = _dbContext.ProblemSolvingTypes;

            query = searchOptions.Apply(query);
            query = sortOptions.Apply(query);

            var size = await query.CountAsync(ct);

            var items = await query
                .Skip(pagingOptions.Offset.Value)
                .Take(pagingOptions.Limit.Value)
                .ProjectTo<ProblemSolvingType>().ToArrayAsync(ct);

            return new PagedResult<ProblemSolvingType>
            {
                Items = items,
                Size = size
            };
        }

        public async Task<ProblemSolvingType> UpdateProblemSolvingTypeAsync(
            Guid Id, ProblemSolvingType problemSolvingTypeForm, CancellationToken ct)
        {
            ProblemSolvingTypeEntity entity = await _dbContext.ProblemSolvingTypes.SingleOrDefaultAsync(i => i.Id == Id, ct);
            if (entity == null) return null;

            // Return if provided name is not valid
            if (!entity.Tag.Equals(problemSolvingTypeForm.Tag, StringComparison.OrdinalIgnoreCase)) return null;

            entity = Mapper.Map<ProblemSolvingTypeEntity>(problemSolvingTypeForm);
            _dbContext.ProblemSolvingTypes.Update(entity);
            var updated = await _dbContext.SaveChangesAsync(ct);
            if (updated < 1) throw new InvalidOperationException("Could not update the problem solving type (8d type)");
            return Mapper.Map<ProblemSolvingType>(entity);
        }
    }
}
