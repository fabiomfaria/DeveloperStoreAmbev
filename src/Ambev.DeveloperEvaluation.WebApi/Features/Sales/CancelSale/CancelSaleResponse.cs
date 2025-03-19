using System;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale
{
    public class CancelSaleResponse
    {
        public Guid SaleId { get; set; }
        public DateTime CancellationDate { get; set; }
        public bool Cancelled { get; set; }
        public string? Message { get; set; }
    }
}