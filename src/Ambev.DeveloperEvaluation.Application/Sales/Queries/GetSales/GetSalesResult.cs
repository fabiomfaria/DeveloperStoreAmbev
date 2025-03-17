using System;
using System.Collections.Generic;

namespace Ambev.DeveloperEvaluation.Application.Sales.Queries.GetSales
{
    public class GetSalesResult
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public List<SaleSummaryDto> Sales { get; set; } = new List<SaleSummaryDto>();
    }

    public class SaleSummaryDto
    {
        public Guid SaleId { get; set; }
        public string SaleNumber { get; set; }
        public DateTime SaleDate { get; set; }
        public string CustomerName { get; set; }
        public string BranchName { get; set; }
        public decimal TotalAmount { get; set; }
        public int ItemCount { get; set; }
        public bool Cancelled { get; set; }
    }
}
