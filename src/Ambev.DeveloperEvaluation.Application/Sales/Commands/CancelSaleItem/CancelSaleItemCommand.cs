using Ambev.DeveloperEvaluation.Application.Sales.Commands.CancelSaleItem;
using MediatR;
using System;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands.CancelSaleItem
{
    public class CancelSaleItemCommand : IRequest<CancelSaleItemResult>
    {
        public Guid SaleId { get; set; }
        public Guid SaleItemId { get; set; }
    }
}