using Ambev.DeveloperEvaluation.Domain.Entities;
using System;

namespace Ambev.DeveloperEvaluation.Domain.Events
{
    public class SaleModifiedEvent
    {
        public Guid SaleId { get; }
        public string SaleNumber { get; }
        public decimal TotalAmount { get; }

        public SaleModifiedEvent(Sale sale)
        {
            SaleId = sale.Id;
            SaleNumber = sale.SaleNumber;
            TotalAmount = sale.TotalAmount;
        }
    }
}