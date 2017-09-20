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
    public interface ICustomerService
    {
        Task<Customer> GetCustomerByIdAsync(
            Guid id,
            CancellationToken ct);

        Task<Customer> GetCustomerByNameAsync(
            string name,
            CancellationToken ct);

        Task<PagedResult<Customer>> GetCustomersAsync(
            PagingOptions pagingOptions,
            SearchOptions<Customer, CustomerEntity> searchOptions,
            SortOptions<Customer, CustomerEntity> sortOptions,
            CancellationToken ct);

        Task<Guid> CreateCustomerAsync(
            Customer customerForm, CancellationToken ct);

        Task<Customer> UpdateCustomerAsync(
            Guid Id, Customer customerForm, CancellationToken ct);

        Task DeleteCustomerByIDAsync(
            Guid Id, CancellationToken ct);
    }

    public class CustomerService: ICustomerService
    {
        private readonly ProblemSolvingDBContext _dbContext;

        public CustomerService(ProblemSolvingDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Customer> GetCustomerByIdAsync(Guid Id, CancellationToken ct)
        {
            CustomerEntity entity = await _dbContext.Customers.SingleOrDefaultAsync(i => i.Id == Id, ct);
            if (entity == null) return null;

            return Mapper.Map<Customer>(entity);
        }

        public async Task<Customer> GetCustomerByNameAsync(string name, CancellationToken ct)
        {
            CustomerEntity entity = await _dbContext.Customers.SingleOrDefaultAsync(i => i.Name.Equals(name, StringComparison.OrdinalIgnoreCase), ct);
            if (entity == null) return null;

            return Mapper.Map<Customer>(entity);
        }

        public async Task<PagedResult<Customer>> GetCustomersAsync(
            PagingOptions pagingOptions,
            SearchOptions<Customer, CustomerEntity> searchOptions,
            SortOptions<Customer, CustomerEntity> sortOptions,
            CancellationToken ct)
        {
            IQueryable<CustomerEntity> query = _dbContext.Customers;

            query = searchOptions.Apply(query);
            query = sortOptions.Apply(query);

            var size = await query.CountAsync(ct);

            var items = await query
                .Skip(pagingOptions.Offset.Value)
                .Take(pagingOptions.Limit.Value)
                .ProjectTo<Customer>().ToArrayAsync(ct);

            return new PagedResult<Customer>
            {
                Items = items,
                Size = size
            };
        }

        public async Task<Guid> CreateCustomerAsync(
            Customer customer, CancellationToken ct)
        {
            var customerId = Guid.NewGuid();

            CustomerEntity entity = Mapper.Map<CustomerEntity>(customer);
            entity.Id = customerId;

            var newCustomer = _dbContext.Customers.Add(entity);
            var created = await _dbContext.SaveChangesAsync(ct);
            if (created < 1) throw new InvalidOperationException("Could not create the customer");

            return customerId;
        }

        public async Task<Customer> UpdateCustomerAsync(
            Guid Id, Customer customer, CancellationToken ct)
        {
            CustomerEntity entity = await _dbContext.Customers.SingleOrDefaultAsync(i => i.Id == Id, ct);
            if (entity == null) return null;

            // Return if provided name is not valid
            if (!entity.Name.Equals(customer.Name, StringComparison.OrdinalIgnoreCase)) return null;

            entity.Location = customer.Location;
            _dbContext.Customers.Update(entity);
            var updated = await _dbContext.SaveChangesAsync(ct);
            if (updated < 1) throw new InvalidOperationException("Could not update the customer");
            return Mapper.Map<Customer>(entity);
        }

        public async Task DeleteCustomerByIDAsync(Guid Id, CancellationToken ct)
        {
            var customer = await _dbContext.Customers.SingleOrDefaultAsync(c => c.Id == Id, ct);
            if (customer == null) return;

            _dbContext.Customers.Remove(customer);
            var deleted = await _dbContext.SaveChangesAsync(ct);
            if (deleted < 1) throw new InvalidOperationException("Could not delete the customer");
        }
    }
}
