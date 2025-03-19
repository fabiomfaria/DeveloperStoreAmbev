using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Entities;
using MongoDB.Driver;

namespace Ambev.DeveloperEvaluation.MongoDB.Repositories
{
    public class CustomerRepository : BaseRepository<Customer>
    {
        private readonly IMongoCollection<Customer> _collection;

        public CustomerRepository(MongoDbContext context) : base(context, "customers")
        {
            _collection = context.GetCollection<Customer>("customers");
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<Customer> GetCustomerByIdAsync(Guid id)
        {
            return await _collection.Find(customer => customer.Id == id).FirstOrDefaultAsync();
        }


        public async Task<IEnumerable<Customer>> GetCustomersWithPaginationAsync(int pageNumber, int pageSize)
        {
            return await _collection.Find(_ => true)
                .Skip((pageNumber - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();
        }

        public async Task CreateCustomerAsync(Customer customer)
        {
            await _collection.InsertOneAsync(customer);
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            await _collection.ReplaceOneAsync(x => x.Id == customer.Id, customer);
        }

        public async Task DeleteCustomerAsync(Guid id)
        {
            await _collection.DeleteOneAsync(customer => customer.Id == id);
        }

        public async Task<bool> CustomerExistsAsync(Guid id)
        {
            var count = await _collection.CountDocumentsAsync(customer => customer.Id == id);
            return count > 0;
        }

        public async Task<long> GetCustomersCountAsync()
        {
            return await _collection.CountDocumentsAsync(_ => true);
        }
    }
}
