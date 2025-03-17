using System;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands.CancelSale
{
    public class CancelSaleResult
    {
        public Guid Id { get; set; }
        public string SaleNumber { get; set; }
        public bool IsCancelled { get; set; }
        public DateTime CancellationDate { get; set; }
    }
}
