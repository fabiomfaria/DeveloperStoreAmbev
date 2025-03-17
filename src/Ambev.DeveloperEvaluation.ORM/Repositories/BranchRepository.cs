using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    public class BranchRepository : IBranchRepository
    {
        private readonly DefaultContext _context;

        public BranchRepository(DefaultContext context)
        {
            _context = context;
        }

        public async Task<Branch> GetByIdAsync(Guid id)
        {
            return await _context.Branches.FindAsync(id);
        }

        public async Task<IEnumerable<Branch>> GetAllAsync()
        {
            return await _context.Branches.ToListAsync();
        }

        public async Task AddAsync(Branch branch)
        {
            await _context.Branches.AddAsync(branch);
        }

        public Task UpdateAsync(Branch branch)
        {
            _context.Entry(branch).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(Guid id)
        {
            var branch = await _context.Branches.FindAsync(id);
            if (branch != null)
            {
                _context.Branches.Remove(branch);
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
