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
    public interface IPlantService
    {
        Task<Plant> GetPlantByTagAsync(string tag, CancellationToken ct);

        Task<PagedResult<Plant>> GetPlantsAsync(
            PagingOptions pagingOptions,
            SearchOptions<Plant, PlantEntity> searchOptions,
            SortOptions<Plant, PlantEntity> sortOptions,
            CancellationToken ct);

        Task<Guid> CreatePlantAsync(
            Plant plantForm, CancellationToken ct);

        Task<Plant> UpdatePlantAsync(
            Guid Id, Plant plantForm, CancellationToken ct);

        Task DeletePlantByIDAsync(Guid Id, CancellationToken ct);
    }

    public class PlantService : IPlantService
    {
        private readonly ProblemSolvingDBContext _dbContext;

        public PlantService(ProblemSolvingDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Plant> GetPlantByTagAsync(string tag, CancellationToken ct)
        {
            PlantEntity entity = await _dbContext.Plants.SingleOrDefaultAsync(i => i.Tag.Equals(tag, StringComparison.OrdinalIgnoreCase), ct);
            if (entity == null) return null;

            return Mapper.Map<Plant>(entity);
        }

        public async Task<PagedResult<Plant>> GetPlantsAsync(
            PagingOptions pagingOptions,
            SearchOptions<Plant, PlantEntity> searchOptions,
            SortOptions<Plant, PlantEntity> sortOptions,
            CancellationToken ct)
        {
            IQueryable<PlantEntity> query = _dbContext.Plants;

            query = searchOptions.Apply(query);
            query = sortOptions.Apply(query);

            var size = await query.CountAsync(ct);

            var items = await query
                .Skip(pagingOptions.Offset.Value)
                .Take(pagingOptions.Limit.Value)
                .ProjectTo<Plant>().ToArrayAsync(ct);

            return new PagedResult<Plant>
            {
                Items = items,
                Size = size
            };
        }

        public async Task<Guid> CreatePlantAsync(Plant plantForm, CancellationToken ct)
        {
            var id = Guid.NewGuid();

            PlantEntity entity = Mapper.Map<PlantEntity>(plantForm);
            entity.Id = id;

            var newObj = _dbContext.Plants.Add(entity);
            var created = await _dbContext.SaveChangesAsync(ct);
            if (created < 1) throw new InvalidOperationException("Could not create the requested object");

            return id;
        }

        public async Task DeletePlantByIDAsync(Guid Id, CancellationToken ct)
        {
            var objToRemove = await _dbContext.Plants.SingleOrDefaultAsync(c => c.Id == Id, ct);
            if (objToRemove == null) return;

            _dbContext.Plants.Remove(objToRemove);
            var deleted = await _dbContext.SaveChangesAsync(ct);
            if (deleted < 1) throw new InvalidOperationException("Could not delete the object");
        }

        public async Task<Plant> UpdatePlantAsync(
            Guid Id, Plant plantForm, CancellationToken ct)
        {
            PlantEntity entity = await _dbContext.Plants.SingleOrDefaultAsync(i => i.Id == Id, ct);
            if (entity == null) return null;

            // Return if provided name is not valid
            if (!entity.Tag.Equals(plantForm.Tag, StringComparison.OrdinalIgnoreCase)) return null;

            entity = Mapper.Map<PlantEntity>(plantForm);
            _dbContext.Plants.Update(entity);
            var updated = await _dbContext.SaveChangesAsync(ct);
            if (updated < 1) throw new InvalidOperationException("Could not update the plant");
            return Mapper.Map<Plant>(entity);
        }
    }
}
