using Ambev.DeveloperEvaluation.Domain.Entities;
using System;

namespace Ambev.DeveloperEvaluation.Domain.Events
{
    public class ItemCancelledEvent
    {
        public Guid SaleId { get; }
        public string SaleNumber { get; }
        public Guid ItemId { get; }
        public string ProductName { get; }
        public int Quantity { get; }
        public decimal TotalAmount { get; }
        public DateTime CancellationDate { get; }

        public ItemCancelledEvent(Sale sale, SaleItem item)
        {
            SaleId = sale.Id;
            SaleNumber = sale.SaleNumber;
            ItemId = item.Id;
            ProductName = item.Product.Name;
            Quantity = item.Quantity;
            TotalAmount = item.TotalAmount;
            CancellationDate = DateTime.UtcNow;
        }
    }
}