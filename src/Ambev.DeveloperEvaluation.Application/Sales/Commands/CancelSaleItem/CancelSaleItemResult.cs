using System;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands.CancelSaleItem
{
    public class CancelSaleItemResult
    {
        public Guid SaleId { get; set; }
        public Guid SaleItemId { get; set; }
        public decimal UpdatedSaleTotal { get; set; }
        public bool Cancelled { get; set; }
    }
}
