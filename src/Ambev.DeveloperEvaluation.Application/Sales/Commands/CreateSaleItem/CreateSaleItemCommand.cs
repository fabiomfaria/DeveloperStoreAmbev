using System;
using System.Collections.Generic;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands.CreateSaleItem
{
    public class CreateSaleItemCommand : IRequest<CreateSaleItemResult>
    {
        public Guid SaleId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal? DiscountPercentage { get; set; }
    }
}
