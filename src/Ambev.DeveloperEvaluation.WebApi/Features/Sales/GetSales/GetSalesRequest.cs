using System;
using System.ComponentModel.DataAnnotations;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSales
{
    public class GetSalesRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "Page number must be greater than 0")]
        public int PageNumber { get; set; } = 1;

        [Range(1, 100, ErrorMessage = "Page size must be between 1 and 100")]
        public int PageSize { get; set; } = 10;

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public Guid? CustomerId { get; set; }

        public Guid? BranchId { get; set; }

        public bool? IncludeCancelled { get; set; }
    }
}