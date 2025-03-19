using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.Queries.GetSales
{
    public class GetSalesHandler : IRequestHandler<GetSalesQuery, GetSalesResult>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;

        public GetSalesHandler(ISaleRepository saleRepository, IMapper mapper)
        {
            _saleRepository = saleRepository ?? throw new ArgumentNullException(nameof(saleRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        // Replace all occurrences of 'SaleDate' with 'Date' in the Handle method
        public async Task<GetSalesResult> Handle(GetSalesQuery request, CancellationToken cancellationToken)
        {
            // Get all sales
            var sales = await _saleRepository.GetAllAsync();

            // Apply filters
            var salesQuery = sales.AsQueryable();

            // Filter by date range
            if (request.StartDate.HasValue)
            {
                salesQuery = salesQuery.Where(s => s.Date >= request.StartDate.Value);
            }

            if (request.EndDate.HasValue)
            {
                salesQuery = salesQuery.Where(s => s.Date <= request.EndDate.Value);
            }

            // Filter by customer
            if (request.CustomerId.HasValue)
            {
                salesQuery = salesQuery.Where(s => s.CustomerId == request.CustomerId.Value);
            }

            // Filter by branch
            if (request.BranchId.HasValue)
            {
                salesQuery = salesQuery.Where(s => s.BranchId == request.BranchId.Value);
            }

            // Filter by cancelled status
            if (request.IncludeCancelled.HasValue)
            {
                bool includeCancelled = request.IncludeCancelled.Value;
                if (!includeCancelled)
                {
                    salesQuery = salesQuery.Where(s => !s.IsCancelled);
                }
            }
            else
            {
                // Default: exclude cancelled
                salesQuery = salesQuery.Where(s => !s.IsCancelled);
            }

            // Order by date descending
            salesQuery = salesQuery.OrderByDescending(s => s.Date);

            // Get total count
            int totalCount = salesQuery.Count();

            // Apply pagination
            var pageSize = request.PageSize <= 0 ? 10 : request.PageSize;
            var pageNumber = request.PageNumber <= 0 ? 1 : request.PageNumber;

            var pagedSales = salesQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Map to DTOs
            var salesDtos = _mapper.Map<List<SaleSummaryDto>>(pagedSales);

            // Create result
            return new GetSalesResult
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling((double)totalCount / pageSize),
                Sales = salesDtos
            };
        }
    }
}
