using System;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSaleItem
{
    public class CancelSaleItemResponse
    {
        public Guid SaleId { get; set; }
        public Guid SaleItemId { get; set; }
        public DateTime CancellationDate { get; set; }
        public bool Cancelled { get; set; }
        public string? Message { get; set; }
        public decimal UpdatedSaleTotal { get; set; }
    }
}
