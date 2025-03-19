using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Entities;
using MongoDB.Driver;

namespace Ambev.DeveloperEvaluation.MongoDB.Repositories
{
    public class BranchRepository : BaseRepository<Branch>
    {
        private readonly IMongoCollection<Branch> _collection;

        public BranchRepository(MongoDbContext context) : base(context, "branches")
        {
            _collection = context.GetCollection<Branch>("branches");
        }

        public async Task<IEnumerable<Branch>> GetAllBranchesAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<Branch> GetBranchByIdAsync(Guid id)
        {
            return await _collection.Find(branch => branch.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateBranchAsync(Branch branch)
        {
            await _collection.InsertOneAsync(branch);
        }

        public async Task UpdateBranchAsync(Branch branch)
        {
            await _collection.ReplaceOneAsync(x => x.Id == branch.Id, branch);
        }

        public async Task DeleteBranchAsync(Guid id)
        {
            await _collection.DeleteOneAsync(branch => branch.Id == id);
        }

        public async Task<bool> BranchExistsAsync(Guid id)
        {
            var count = await _collection.CountDocumentsAsync(branch => branch.Id == id);
            return count > 0;
        }
    }
}
