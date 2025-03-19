using System;
using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands.CreateSaleItem
{
    public class CreateSaleItemHandler : IRequestHandler<CreateSaleItemCommand, CreateSaleItemResult>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IProductRepository _productRepository;
        private readonly IEventPublisher _eventPublisher;
        private readonly ILogger<CreateSaleItemHandler> _logger;

        public CreateSaleItemHandler(
            ISaleRepository saleRepository,
            IProductRepository productRepository,
            IEventPublisher eventPublisher,
            ILogger<CreateSaleItemHandler> logger)
        {
            _saleRepository = saleRepository;
            _productRepository = productRepository;
            _eventPublisher = eventPublisher;
            _logger = logger;
        }

        public async Task<CreateSaleItemResult> Handle(CreateSaleItemCommand request, CancellationToken cancellationToken)
        {
            // Validate sale exists
            var sale = await _saleRepository.GetByIdAsync(request.SaleId);
            if (sale == null)
            {
                throw new ArgumentException($"Sale with ID {request.SaleId} not found");
            }

            if (sale.IsCancelled)
            {
                throw new InvalidOperationException("Cannot add items to a cancelled sale");
            }

            // Validate product exists
            var product = await _productRepository.GetByIdAsync(request.ProductId);
            if (product == null)
            {
                throw new ArgumentException($"Product with ID {request.ProductId} not found");
            }

            // Apply business rules for discount
            decimal discountPercentage = CalculateDiscountPercentage(request.Quantity, request.DiscountPercentage);
            decimal unitPrice = request.UnitPrice > 0 ? request.UnitPrice : product.Price;
            decimal discountAmount = (unitPrice * request.Quantity) * (discountPercentage / 100);
            decimal totalAmount = (unitPrice * request.Quantity) - discountAmount;

            // Create sale item
            var saleItem = new SaleItem(
                sale: sale,
                product: product,
                quantity: request.Quantity,
                unitPrice: unitPrice,
                discount: discountPercentage,
                totalAmount: totalAmount
            );

            // Add item to sale
            sale.Items.Add(saleItem);

            // Update sale in repository
            await _saleRepository.UpdateAsync(sale);


            _logger.LogInformation($"Sale item created for sale {sale.Id}, product {product.Id}, quantity {request.Quantity}");

            // Return result
            return new CreateSaleItemResult
            {
                Id = saleItem.Id,
                SaleId = saleItem.SaleId,
                ProductId = saleItem.ProductId,
                ProductName = product.Name,
                Quantity = saleItem.Quantity,
                UnitPrice = saleItem.UnitPrice,
                DiscountPercentage = saleItem.Discount,
                DiscountAmount = discountAmount,
                TotalAmount = saleItem.TotalAmount,
                IsCancelled = saleItem.IsCancelled
            };
        }

        private decimal CalculateDiscountPercentage(int quantity, decimal? requestedDiscount)
        {
            // Business rules:
            // - Purchases above 4 identical items have a 10% discount
            // - Purchases between 10 and 20 identical items have a 20% discount
            // - Purchases below 4 items cannot have a discount

            if (quantity < 4)
                return 0; // No discount allowed

            if (quantity >= 10 && quantity <= 20)
                return requestedDiscount.HasValue && requestedDiscount.Value <= 20 ? requestedDiscount.Value : 20;

            if (quantity >= 4 && quantity < 10)
                return requestedDiscount.HasValue && requestedDiscount.Value <= 10 ? requestedDiscount.Value : 10;

            return 0; // Default case (should not reach here due to validator)
        }
    }
}
