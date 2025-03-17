using Ambev.DeveloperEvaluation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Repositories
{
    public interface ISaleRepository
    {
        Task<Sale> GetByIdAsync(Guid id);
        Task<Sale> GetByNumberAsync(string saleNumber);
        Task<IEnumerable<Sale>> GetAllAsync();
        Task<IEnumerable<Sale>> GetByCustomerIdAsync(Guid customerId);
        Task<IEnumerable<Sale>> GetByBranchIdAsync(Guid branchId);
        Task<IEnumerable<Sale>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task AddAsync(Sale sale);
        Task UpdateAsync(Sale sale);
        Task SaveChangesAsync();
    }
}