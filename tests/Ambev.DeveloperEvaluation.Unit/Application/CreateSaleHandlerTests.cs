using System;
using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Application.Commands;
using Ambev.DeveloperEvaluation.Application.Handlers;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Interfaces;
using Ambev.DeveloperEvaluation.Domain.Validation;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using MediatR;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application
{
    public class CreateSaleHandlerTests
    {
        private readonly ISaleRepository _saleRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IBranchRepository _branchRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ISaleValidator _saleValidator;
        private readonly ISaleItemValidator _saleItemValidator;
        private readonly CreateSaleHandler _handler;
        
        public CreateSaleHandlerTests()
        {
            _saleRepository = Substitute.For<ISaleRepository>();
            _customerRepository = Substitute.For<ICustomerRepository>();
            _branchRepository = Substitute.For<IBranchRepository>();
            _productRepository = Substitute.For<IProductRepository>();
            _mapper = Substitute.For<IMapper>();
            _mediator = Substitute.For<IMediator>();
            _saleValidator = Substitute.For<ISaleValidator>();
            _saleItemValidator = Substitute.For<ISaleItemValidator>();
            
            _handler = new CreateSaleHandler(
                _saleRepository,
                _customerRepository,
                _branchRepository,
                _productRepository,
                _mapper,
                _mediator,
                _saleValidator,
                _saleItemValidator
            );
        }
        
        [Fact]
        public async Task Handle_ValidRequest_ShouldCreateSaleAndReturnId()
        {
            // Arrange
            var request = SaleHandlersTestData.GetValidCreateSaleCommand();
            
            _customerRepository.GetByIdAsync(Arg.Any<Guid>())
                .Returns(new Customer(Guid.NewGuid(), "Test Customer", "test@example.com", "123456789"));
                
            _branchRepository.GetByIdAsync(Arg.Any<Guid>())
                .Returns(new Branch(Guid.NewGuid(), "Test Branch", "Test Location"));
                
            _productRepository.GetByIdAsync(Arg.Any<Guid>())
                .Returns(new Product(Guid.NewGuid(), "Test Product", 100m));
                
            _saleValidator.ValidateAsync(Arg.Any<Sale>())
                .Returns(new ValidationResult { IsValid = true });
                
            _saleItemValidator.ValidateAsync(Arg.Any<SaleItem>())
                .Returns(new ValidationResult { IsValid = true });
                
            _saleRepository.AddAsync(Arg.Any<Sale>())
                .Returns(Task.CompletedTask);
                
            // Act
            var result = await _handler.Handle(request, CancellationToken.None);
            
            // Assert
            Assert.NotEqual(Guid.Empty, result);
            await _saleRepository.Received(1).AddAsync(Arg.Any<Sale>());
            await _mediator.Received(1).Publish(Arg.Any<SaleCreatedEvent>(), Arg.Any<CancellationToken>());
        }
        
        [Fact]
        public async Task Handle_CustomerNotFound_ShouldThrowException()
        {
            // Arrange
            var request = SaleHandlersTestData.GetValidCreateSaleCommand();
            
            _customerRepository.GetByIdAsync(Arg.Any<Guid>())
                .ReturnsNull();
                
            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _handler.Handle(request, CancellationToken.None)
            );
            
            Assert.Contains("Customer not found", exception.Message);
            await _saleRepository.DidNotReceive().AddAsync(Arg.Any<Sale>());
        }
        
        [Fact]
        public async Task Handle_BranchNotFound_ShouldThrowException()
        {
            // Arrange
            var request = SaleHandlersTestData.GetValidCreateSaleCommand();
            
            _customerRepository.GetByIdAsync(Arg.Any<Guid>())
                .Returns(new Customer(Guid.NewGuid(), "Test Customer", "test@example.com", "123456789"));
                
            _branchRepository.GetByIdAsync(Arg.Any<Guid>())
                .ReturnsNull();
                
            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _handler.Handle(request, CancellationToken.None)
            );
            
            Assert.Contains("Branch not found", exception.Message);
            await _saleRepository.DidNotReceive().AddAsync(Arg.Any<Sale>());
        }
        
        [Fact]
        public async Task Handle_ProductNotFound_ShouldThrowException()
        {
            // Arrange
            var request = SaleHandlersTestData.GetValidCreateSaleCommand();
            
            _customerRepository.GetByIdAsync(Arg.Any<Guid>())
                .Returns(new Customer(Guid.NewGuid(), "Test Customer", "test@example.com", "123456789"));
                
            _branchRepository.GetByIdAsync(Arg.Any<Guid>())
                .Returns(new Branch(Guid.NewGuid(), "Test Branch", "Test Location"));
                
            _productRepository.GetByIdAsync(Arg.Any<Guid>())
                .ReturnsNull();
                
            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _handler.Handle(request, CancellationToken.None)
            );
            
            Assert.Contains("Product not found", exception.Message);
            await _saleRepository.DidNotReceive().AddAsync(Arg.Any<Sale>());
        }
        
        [Fact]
        public async Task Handle_InvalidSale_ShouldThrowException()
        {
            // Arrange
            var request = SaleHandlersTestData.GetValidCreateSaleCommand();
            
            _customerRepository.GetByIdAsync(Arg.Any<Guid>())
                .Returns(new Customer(Guid.NewGuid(), "Test Customer", "test@example.com", "123456789"));
                
            _branchRepository.GetByIdAsync(Arg.Any<Guid>())
                .Returns(new Branch(Guid.NewGuid(), "Test Branch", "Test Location"));
                
            _productRepository.GetByIdAsync(Arg.Any<Guid>())
                .Returns(new Product(Guid.NewGui