using Ambev.DeveloperEvaluation.Domain.Common;
using System;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class SaleItem : BaseEntity
    {
        public Sale Sale { get; private set; }
        public Guid SaleId { get; private set; }
        public Product Product { get; private set; }
        public Guid ProductId { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }
        public decimal Discount { get; private set; }
        public decimal TotalAmount { get; private set; }
        public bool IsCancelled { get; private set; }

        // For EF
        protected SaleItem() { }

        public SaleItem(Sale sale, Product product, int quantity, decimal unitPrice, decimal discount, decimal totalAmount)
        {
            Sale = sale;
            SaleId = sale.Id;
            Product = product;
            ProductId = product.Id;
            Quantity = quantity;
            UnitPrice = unitPrice;
            Discount = discount;
            TotalAmount = totalAmount;
            IsCancelled = false;
        }

        public void Cancel()
        {
            if (!IsCancelled)
            {
                IsCancelled = true;
                TotalAmount = 0;
            }
        }
    }
}