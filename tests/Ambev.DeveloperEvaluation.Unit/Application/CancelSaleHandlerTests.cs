using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Application.Commands;
using Ambev.DeveloperEvaluation.Application.Commands.Handlers;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Interfaces;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application
{
    public class CancelSaleHandlerTests
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEventPublisher _eventPublisher;
        private readonly CancelSaleHandler _handler;

        public CancelSaleHandlerTests()
        {
            _saleRepository = Substitute.For<ISaleRepository>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _eventPublisher = Substitute.For<IEventPublisher>();
            _handler = new CancelSaleHandler(
                _saleRepository,
                _unitOfWork,
                _eventPublisher);
        }

        [Fact]
        public async Task Handle_WithValidCommand_ShouldCancelSaleAndPublishEvent()
        {
            // Arrange
            var command = new CancelSaleCommand(Guid.NewGuid());
            var sale = CancelSaleHandlerTestData.GetActiveSale();

            _saleRepository.GetByIdAsync(command.Id).Returns(sale);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(sale.Cancelled);
            
            await _saleRepository.Received(1).GetByIdAsync(command.Id);
            await _saleRepository.Received(1).UpdateAsync(sale);
            await _unitOfWork.Received(1).SaveChangesAsync();
            await _eventPublisher.Received(1).PublishAsync("sale.cancelled", Arg.Any<object>());
        }

        [Fact]
        public async Task Handle_WithNonExistingSale_ShouldThrowNotFoundException()
        {
            // Arrange
            var command = new CancelSaleCommand(Guid.NewGuid());
            _saleRepository.GetByIdAsync(command.Id).Returns((Sale)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => 
                _handler.Handle(command, CancellationToken.None));
            
            await _saleRepository.Received(1).GetByIdAsync(command.Id);
            await _saleRepository.DidNotReceive().UpdateAsync(Arg.Any<Sale>());
            await _unitOfWork.DidNotReceive().SaveChangesAsync();
        }

        [Fact]
        public async Task Handle_WithAlreadyCancelledSale_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var command = new CancelSaleCommand(Guid.NewGuid());
            var sale = CancelSaleHandlerTestData.GetCancelledSale();

            _saleRepository.GetByIdAsync(command.Id).Returns(sale);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => 
                _handler.Handle(command, CancellationToken.None));
            
            await _saleRepository.Received(1).GetByIdAsync(command.Id);
            await _saleRepository.DidNotReceive().UpdateAsync(Arg.Any<Sale>());
            await _unitOfWork.DidNotReceive().SaveChangesAsync();
        }
    }
}
