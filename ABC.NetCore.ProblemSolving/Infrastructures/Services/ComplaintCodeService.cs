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
    public interface IComplaintCodeService
    {
        Task<ComplaintCode> GetComplaintCodeByCodeAsync(string code, CancellationToken ct);

        Task<PagedResult<ComplaintCode>> GetComplaintCodesAsync(
            PagingOptions pagingOptions,
            SearchOptions<ComplaintCode, ComplaintCodeEntity> searchOptions,
            SortOptions<ComplaintCode, ComplaintCodeEntity> sortOptions,
            CancellationToken ct);

        Task<Guid> CreateComplaintCodeAsync(
            ComplaintCode complaintCodeForm, CancellationToken ct);

        Task<ComplaintCode> UpdateComplaintCodeByIDAsync(
            Guid Id, ComplaintCode complaintCodeForm, CancellationToken ct);

        Task DeleteComplaintCodeByIDAsync(Guid Id, CancellationToken ct);
    }

    public class ComplaintCodeService: IComplaintCodeService
    {
        private readonly ProblemSolvingDBContext _dbContext;

        public ComplaintCodeService(ProblemSolvingDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ComplaintCode> GetComplaintCodeByCodeAsync(string code, CancellationToken ct)
        {
            ComplaintCodeEntity entity = await _dbContext.ComplaintCodes.SingleOrDefaultAsync(c => c.Code.Equals(code), ct);
            if (entity == null) return null;

            return Mapper.Map<ComplaintCode>(entity);
        }

        public async Task<Guid> CreateComplaintCodeAsync(ComplaintCode complaintCodeForm, CancellationToken ct)
        {
            var id = Guid.NewGuid();

            ComplaintCodeEntity entity = Mapper.Map<ComplaintCodeEntity>(complaintCodeForm);
            entity.Id = id;

            var newObj = _dbContext.ComplaintCodes.Add(entity);
            var created = await _dbContext.SaveChangesAsync(ct);
            if (created < 1) throw new InvalidOperationException("Could not create the complaint code");

            return id;
        }

        public async Task DeleteComplaintCodeByIDAsync(Guid Id, CancellationToken ct)
        {
            var objToRemove = await _dbContext.ComplaintCodes.SingleOrDefaultAsync(c => c.Id == Id, ct);
            if (objToRemove == null) return;

            _dbContext.ComplaintCodes.Remove(objToRemove);
            var deleted = await _dbContext.SaveChangesAsync(ct);
            if (deleted < 1) throw new InvalidOperationException("Could not delete the object");
        }

        public async Task<PagedResult<ComplaintCode>> GetComplaintCodesAsync(
            PagingOptions pagingOptions,
            SearchOptions<ComplaintCode, ComplaintCodeEntity> searchOptions,
            SortOptions<ComplaintCode, ComplaintCodeEntity> sortOptions,
            CancellationToken ct)
        {
            IQueryable<ComplaintCodeEntity> query = _dbContext.ComplaintCodes;

            query = searchOptions.Apply(query);
            query = sortOptions.Apply(query);

            var size = await query.CountAsync(ct);

            var items = await query
                .Skip(pagingOptions.Offset.Value)
                .Take(pagingOptions.Limit.Value)
                .ProjectTo<ComplaintCode>().ToArrayAsync(ct);

            return new PagedResult<ComplaintCode>
            {
                Items = items,
                Size = size
            };
        }

        public async Task<ComplaintCode> UpdateComplaintCodeByIDAsync(
            Guid Id, ComplaintCode complaintCodeForm, CancellationToken ct)
        {
            ComplaintCodeEntity entity = await _dbContext.ComplaintCodes.SingleOrDefaultAsync(i => i.Id == Id, ct);
            if (entity == null) return null;

            // Return if provided name is not valid
            if (entity.GroupCode != complaintCodeForm.GroupCode || entity.Code != complaintCodeForm.Code) return null;

            entity = Mapper.Map<ComplaintCodeEntity>(complaintCodeForm);
            _dbContext.ComplaintCodes.Update(entity);
            var updated = await _dbContext.SaveChangesAsync(ct);
            if (updated < 1) throw new InvalidOperationException("Something went wrong and could not update the provided complaint code.");
            return Mapper.Map<ComplaintCode>(entity);
        }
    }
}
