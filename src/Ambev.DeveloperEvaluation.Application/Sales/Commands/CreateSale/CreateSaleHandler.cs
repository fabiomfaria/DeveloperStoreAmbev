using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Services;
using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands.CreateSale
{
    public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IBranchRepository _branchRepository;
        private readonly IProductRepository _productRepository;
        private readonly IEventPublisher _eventPublisher;
        private readonly IMapper _mapper;

        public CreateSaleHandler(
            ISaleRepository saleRepository,
            ICustomerRepository customerRepository,
            IBranchRepository branchRepository,
            IProductRepository productRepository,
            IEventPublisher eventPublisher,
            IMapper mapper)
        {
            _saleRepository = saleRepository;
            _customerRepository = customerRepository;
            _branchRepository = branchRepository;
            _productRepository = productRepository;
            _eventPublisher = eventPublisher;
            _mapper = mapper;
        }

        public async Task<CreateSaleResult> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
        {
            // Search for the customer and the branch
            var customer = await _customerRepository.GetByIdAsync(request.CustomerId);
            if (customer == null)
                throw new InvalidOperationException($"Customer with ID {request.CustomerId} not found.");

            var branch = await _branchRepository.GetByIdAsync(request.BranchId);
            if (branch == null)
                throw new InvalidOperationException($"Branch with ID {request.BranchId} not found.");

            // Generate sales number
            var saleNumber = $"SALE-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8)}";

            // Create the sale
            var sale = new Sale(saleNumber, request.Date, customer, branch);

            // Add items for sale
            foreach (var itemDto in request.Items)
            {
                var product = await _productRepository.GetByIdAsync(itemDto.ProductId);
                if (product == null)
                    throw new InvalidOperationException($"Product with ID {itemDto.ProductId} not found.");

                sale.AddItem(product, itemDto.Quantity, itemDto.UnitPrice);
            }

            // Save the sale
            await _saleRepository.AddAsync(sale);
            await _saleRepository.SaveChangesAsync();

            // Publish event
            await _eventPublisher.PublishAsync(new SaleCreatedEvent(sale));

            // Map result
            return new CreateSaleResult
            {
                Id = sale.Id,
                SaleNumber = sale.SaleNumber,
                Date = sale.Date,
                CustomerName = customer.Name,
                BranchName = branch.Name,
                TotalAmount = sale.TotalAmount
            };
        }
    }
}