using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly DefaultContext _context;

        public SaleRepository(DefaultContext context)
        {
            _context = context;
        }

        public async Task<Sale> GetByIdAsync(Guid id)
        {
            return await _context.Sales
                .Include(s => s.Customer)
                .Include(s => s.Branch)
                .Include(s => s.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Sale> GetByNumberAsync(string saleNumber)
        {
            return await _context.Sales
                .Include(s => s.Customer)
                .Include(s => s.Branch)
                .Include(s => s.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(s => s.SaleNumber == saleNumber);
        }

        public async Task<IEnumerable<Sale>> GetAllAsync()
        {
            return await _context.Sales
                .Include(s => s.Customer)
                .Include(s => s.Branch)
                .Include(s => s.Items)
                .ThenInclude(i => i.Product)
                .ToListAsync();
        }

        public async Task<IEnumerable<Sale>> GetByCustomerIdAsync(Guid customerId)
        {
            return await _context.Sales
                .Include(s => s.Customer)
                .Include(s => s.Branch)
                .Include(s => s.Items)
                .ThenInclude(i => i.Product)
                .Where(s => s.CustomerId == customerId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Sale>> GetByBranchIdAsync(Guid branchId)
        {
            return await _context.Sales
                .Include(s => s.Customer)
                .Include(s => s.Branch)
                .Include(s => s.Items)
                .ThenInclude(i => i.Product)
                .Where(s => s.BranchId == branchId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Sale>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Sales
                .Include(s => s.Customer)
                .Include(s => s.Branch)
                .Include(s => s.Items)
                .ThenInclude(i => i.Product)
                .Where(s => s.Date >= startDate && s.Date <= endDate)
                .ToListAsync();
        }

        public async Task AddAsync(Sale sale)
        {
            await _context.Sales.AddAsync(sale);
        }

        public Task UpdateAsync(Sale sale)
        {
            _context.Entry(sale).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
