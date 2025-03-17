using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale
{
    public class CreateSaleRequest
    {
        [Required]
        public DateTime SaleDate { get; set; }

        [Required]
        public Guid CustomerId { get; set; }

        [Required]
        public string CustomerName { get; set; }

        [Required]
        public Guid BranchId { get; set; }

        [Required]
        public string BranchName { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "At least one item is required")]
        public List<CreateSaleItemRequest> Items { get; set; } = new List<CreateSaleItemRequest>();
    }

    public class CreateSaleItemRequest
    {
        [Required]
        public Guid ProductId { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Required]
        [Range(1, 20, ErrorMessage = "Quantity must be between 1 and 20")]
        public int Quantity { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Unit price must be greater than 0")]
        public decimal UnitPrice { get; set; }
    }
}