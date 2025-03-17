using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands.CancelSaleItem
{
    public class CancelSaleItemHandler : IRequestHandler<CancelSaleItemCommand, CancelSaleItemResult>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public CancelSaleItemHandler(ISaleRepository saleRepository, IMapper mapper, IMediator mediator)
        {
            _saleRepository = saleRepository ?? throw new ArgumentNullException(nameof(saleRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<CancelSaleItemResult> Handle(CancelSaleItemCommand request, CancellationToken cancellationToken)
        {
            // Retrieve the sale
            var sale = await _saleRepository.GetByIdAsync(request.SaleId, cancellationToken);

            if (sale == null)
            {
                throw new DomainException($"Sale with ID {request.SaleId} not found.");
            }

            // Check if the sale is already cancelled
            if (sale.Cancelled)
            {
                throw new DomainException($"Cannot cancel item: Sale with ID {request.SaleId} is already cancelled.");
            }

            // Find the specific item
            var item = sale.Items.FirstOrDefault(i => i.Id == request.SaleItemId);

            if (item == null)
            {
                throw new DomainException($"Item with ID {request.SaleItemId} not found in Sale with ID {request.SaleId}.");
            }

            // Check if the item is already cancelled
            if (item.Cancelled)
            {
                throw new DomainException($"Item with ID {request.SaleItemId} is already cancelled.");
            }

            // Cancel the item
            item.Cancel();

            // Recalculate the sale total
            sale.RecalculateTotal();

            // Update the sale in the repository
            await _saleRepository.UpdateAsync(sale, cancellationToken);

            // Publish the ItemCancelled event
            await _mediator.Publish(new ItemCancelledEvent
            {
                SaleId = sale.Id,
                ItemId = item.Id,
                ProductId = item.ProductId,
                Quantity = item.Quantity
            }, cancellationToken);

            // Map and return the result
            return new CancelSaleItemResult
            {
                SaleId = sale.Id,
                SaleItemId = item.Id,
                UpdatedSaleTotal = sale.TotalAmount,
                Cancelled = item.Cancelled
            };
        }
    }
}