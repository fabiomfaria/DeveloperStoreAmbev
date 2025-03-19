using System;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSaleItem
{
    public class CreateSaleItemRequest
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal? DiscountPercentage { get; set; }
    }
}
