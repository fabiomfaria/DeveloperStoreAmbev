using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Services;
using AutoMapper;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands.UpdateSale
{
    public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, UpdateSaleResult>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IProductRepository _productRepository;
        private readonly IEventPublisher _eventPublisher;
        private readonly IMapper _mapper;

        public UpdateSaleHandler(
            ISaleRepository saleRepository,
            IProductRepository productRepository,
            IEventPublisher eventPublisher,
            IMapper mapper)
        {
            _saleRepository = saleRepository;
            _productRepository = productRepository;
            _eventPublisher = eventPublisher;
            _mapper = mapper;
        }

        public async Task<UpdateSaleResult> Handle(UpdateSaleCommand request, CancellationToken cancellationToken)
        {
            var sale = await _saleRepository.GetByIdAsync(request.Id);
            if (sale == null)
                throw new InvalidOperationException($"Sale with ID {request.Id} not found.");

            if (sale.IsCancelled)
                throw new InvalidOperationException("It is not possible to modify a canceled sale.");

            // Remove items that are not in the request
            var itemsToRemove = sale.Items
                .Where(i => !request.Items.Any(ri => ri.Id.HasValue && ri.Id.Value == i.Id))
                .ToList();

            foreach (var item in itemsToRemove)
            {
                sale.RemoveItem(item.Id);
            }

            // Add or update items
            foreach (var itemDto in request.Items)
            {
                var product = await _productRepository.GetByIdAsync(itemDto.ProductId);
                if (product == null)
                    throw new InvalidOperationException($"Product with ID {itemDto.ProductId} not found.");

                if (itemDto.Id.HasValue && sale.Items.Any(i => i.Id == itemDto.Id.Value))
                {
                    // Update existing item
                    sale.RemoveItem(itemDto.Id.Value);
                    sale.AddItem(product, itemDto.Quantity, itemDto.UnitPrice);
                }
                else
                {
                    // Add new item
                    sale.AddItem(product, itemDto.Quantity, itemDto.UnitPrice);
                }
            }

            // Save changes
            await _saleRepository.UpdateAsync(sale);
            await _saleRepository.SaveChangesAsync();

            // Publish event
            await _eventPublisher.PublishAsync(new SaleModifiedEvent(sale));

            // Map result
            return new UpdateSaleResult
            {
                Id = sale.Id,
                SaleNumber = sale.SaleNumber,
                Date = sale.Date,
                CustomerName = sale.Customer.Name,
                BranchName = sale.Branch.Name,
                TotalAmount = sale.TotalAmount
            };
        }
    }
}
