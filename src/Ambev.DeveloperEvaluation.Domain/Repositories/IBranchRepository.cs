﻿using Ambev.DeveloperEvaluation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Repositories
{
    public interface IBranchRepository
    {
        Task<Branch> GetByIdAsync(Guid id);
        Task<IEnumerable<Branch>> GetAllAsync();
        Task AddAsync(Branch branch);
        Task UpdateAsync(Branch branch);
        Task DeleteAsync(Guid id);
        Task SaveChangesAsync();
    }
}