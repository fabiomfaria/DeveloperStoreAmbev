using System;
using System.Collections.Generic;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSales
{
    public class GetSalesResponse
    {
        public Guid Id { get; set; }
        public int SaleNumber { get; set; }
        public DateTime SaleDate { get; set; }
        public CustomerDto Customer { get; set; }
        public decimal TotalAmount { get; set; }
        public BranchDto Branch { get; set; }
        public List<SaleItemDto> Items { get; set; }
        public bool IsCancelled { get; set; }
    }

    public class CustomerDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }

    public class BranchDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
    }

    public class SaleItemDto
    {
        public Guid Id { get; set; }
        public ProductDto Product { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal TotalPrice { get; set; }
        public bool IsCancelled { get; set; }
    }

    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SKU { get; set; }
    }
}