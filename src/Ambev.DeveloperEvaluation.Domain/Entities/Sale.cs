using Ambev.DeveloperEvaluation.Domain.Common;
using System;
using System.Collections.Generic;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Sale : BaseEntity
    {
        public string SaleNumber { get; private set; }
        public DateTime Date { get; private set; }
        public Customer Customer { get; private set; }
        public Guid CustomerId { get; private set; }
        public Branch Branch { get; private set; }
        public Guid BranchId { get; private set; }
        public decimal TotalAmount { get; private set; }
        public bool IsCancelled { get; private set; }
        public ICollection<SaleItem> Items { get; private set; } = new List<SaleItem>();

        // For EF
        protected Sale() { }

        public Sale(string saleNumber, DateTime date, Customer customer, Branch branch)
        {
            SaleNumber = saleNumber;
            Date = date;
            Customer = customer;
            CustomerId = customer.Id;
            Branch = branch;
            BranchId = branch.Id;
            IsCancelled = false;
            CalculateTotalAmount();
        }

        public void AddItem(Product product, int quantity, decimal unitPrice)
        {
            // Check maximum item limit
            if (quantity > 20)
            {
                throw new DomainException("You cannot sell more than 20 identical items.");
            }

            // Calculates discount based on quantity
            decimal discount = CalculateDiscount(quantity, unitPrice);

            // Calculates item total
            decimal totalAmount = (quantity * unitPrice) - discount;

            // Add item
            var saleItem = new SaleItem(this, product, quantity, unitPrice, discount, totalAmount);
            Items.Add(saleItem);

            // Recalculates total sales
            CalculateTotalAmount();
        }

        public void RemoveItem(Guid itemId)
        {
            var item = Items.FirstOrDefault(i => i.Id == itemId);
            if (item == null)
            {
                throw new DomainException("Item not found for sale.");
            }

            Items.Remove(item);
            CalculateTotalAmount();
        }

        public void CancelItem(Guid itemId)
        {
            var item = Items.FirstOrDefault(i => i.Id == itemId);
            if (item == null)
            {
                throw new DomainException("Item not found for sale.");
            }

            item.Cancel();
            CalculateTotalAmount();
        }

        public void Cancel()
        {
            if (IsCancelled)
            {
                throw new DomainException("This sale is now cancelled.");
            }

            IsCancelled = true;

            // Cancels all items
            foreach (var item in Items)
            {
                item.Cancel();
            }

            CalculateTotalAmount();
        }

        private decimal CalculateDiscount(int quantity, decimal unitPrice)
        {
            decimal discount = 0;

            // Apply discount rules
            if (quantity >= 10 && quantity <= 20)
            {
                // 20% discount for purchases between 10 and 20 items
                discount = unitPrice * quantity * 0.2m;
            }
            else if (quantity >= 4)
            {
                // 10% discount for purchases of 4 or more items
                discount = unitPrice * quantity * 0.1m;
            }

            return discount;
        }

        private void CalculateTotalAmount()
        {
            TotalAmount = Items.Where(i => !i.IsCancelled).Sum(i => i.TotalAmount);
        }
    }
}