using Ambev.DeveloperEvaluation.Domain.Entities;
using System;

namespace Ambev.DeveloperEvaluation.Domain.Events
{
    public class SaleCreatedEvent
    {
        public Guid SaleId { get; }
        public string SaleNumber { get; }
        public DateTime Date { get; }
        public string CustomerName { get; }
        public string BranchName { get; }
        public decimal TotalAmount { get; }

        public SaleCreatedEvent(Sale sale)
        {
            SaleId = sale.Id;
            SaleNumber = sale.SaleNumber;
            Date = sale.Date;
            CustomerName = sale.Customer.Name;
            BranchName = sale.Branch.Name;
            TotalAmount = sale.TotalAmount;
        }
    }
}