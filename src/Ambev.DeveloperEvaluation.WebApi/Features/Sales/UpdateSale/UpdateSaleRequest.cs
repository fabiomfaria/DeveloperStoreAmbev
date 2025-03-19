using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale
{
    public class UpdateSaleRequest
    {
        [Required]
        public Guid SaleId { get; set; }
        
        public Guid? CustomerId { get; set; }
        
        public Guid? BranchId { get; set; }
        
        public List<UpdateSaleItemRequest>? Items { get; set; }
    }
    
    public class UpdateSaleItemRequest
    {
        public Guid? Id { get; set; }
        
        [Required]
        public Guid ProductId { get; set; }
        
        [Required]
        [Range(1, 20, ErrorMessage = "Quantity must be between 1 and 20")]
        public int Quantity { get; set; }
        
        public decimal? UnitPrice { get; set; }
    }
}
