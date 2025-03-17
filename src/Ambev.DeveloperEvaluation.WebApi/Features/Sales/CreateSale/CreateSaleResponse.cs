using System;
using System.Collections.Generic;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale
{
    public class CreateSaleResponse
    {
        public Guid SaleId { get; set; }
        public string SaleNumber { get; set; }
        public DateTime SaleDate { get; set; }
        public string CustomerName { get; set; }
        public string BranchName { get; set; }
        public decimal TotalAmount { get; set; }
        public List<CreateSaleItemResponse> Items { get; set; } = new List<CreateSaleItemResponse>();
    }

    public class CreateSaleItemResponse
    {
        public Guid ItemId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
