using System;
using System.Collections.Generic;
using System.Linq;
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
    public class CancelSaleItemHandlerTests
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEventPublisher _eventPublisher;
        private readonly CancelSaleItemHandler _handler;

        public CancelSaleItemHandlerTests()
        {
            _saleRepository = Substitute.For<ISaleRepository>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _eventPublisher = Substitute.For<IEventPublisher>();
            _handler = new CancelSaleItemHandler(
                _saleRepository,
                _unitOfWork,
                _eventPublisher);
        }

        [Fact]
        public async Task Handle_WithValidCommand_ShouldCancelItemAndPublishEvent()
        {
            // Arrange
            var saleId = Guid.NewGuid();
            var itemId = Guid.NewGuid();
            var command = new CancelSaleItemCommand(saleId, itemId);
            var sale = CancelSaleItemHandlerTestData.GetSaleWithItems();
            
            // Ensuring the test item exists in the sale
            sale.Items.First().Id = itemId;

            _saleRepository.GetByIdAsync(saleId).Returns(sale);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            var cancelledItem = sale.Items.FirstOrDefault(i => i.Id == itemId);
            Assert.NotNull(cancelledItem);
            Assert.True(cancelledItem.Cancelled);
            
            await _saleRepository.Received(1).GetByIdAsync(saleId);
            await _saleRepository.Received(1).UpdateAsync(sale);
            await _unitOfWork.Received(1).SaveChangesAsync();
            await _eventPublisher.Received(1).PublishAsync("sale.item.cancelled", Arg.Any<object>());
        }

        [Fact]
        public async Task Handle_WithNonExistingSale_ShouldThrowNotFoundException()
        {
            // Arrange
            var command = new CancelSaleItemCommand(Guid.NewGuid(), Guid.NewGuid());
            _saleRepository.GetByIdAsync(command.SaleId).Returns((Sale)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => 
                _handler.Handle(command, CancellationToken.None));
            
            await _saleRepository.Received(1).GetByIdAsync(command.SaleId);
            await _saleRepository.DidNotReceive().UpdateAsync(Arg.Any<Sale>());
            await _unitOfWork.DidNotReceive().SaveChangesAsync();
        }

        [Fact]
        public async Task Handle_WithNonExistingItem_ShouldThrowNotFoundException()
        {
            // Arrange
            var command = new CancelSaleItemCommand(Guid.NewGuid(), Guid.NewGuid());
            var sale = CancelSaleItemHandlerTestData.GetSaleWithItems();
            
            _saleRepository.GetByIdAsync(command.SaleId).Returns(sale);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => 
                _handler.Handle(command, CancellationToken.None));
            
            await _saleRepository.Received(1).GetByIdAsync(command.SaleId);
            await _saleRepository.DidNotReceive().UpdateAsync(Arg.Any<Sale>());
            await _unitOfWork.DidNotReceive().SaveChangesAsync();
        }

        [Fact]
        public async Task Handle_WithAlreadyCancelledItem_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var saleId = Guid.NewGuid();
            var itemId = Guid.NewGuid();
            var command = new CancelSaleItemCommand(saleId, itemId);
            var sale = CancelSaleItemHandlerTestData.GetSaleWithCancelledItem();
            
            // Ensuring the test item exists and is cancelled
            var item = sale.Items.First();
            item.Id = itemId;
            item.Cancelled = true;

            _saleRepository.GetByIdAsync(saleId).Returns(sale);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => 
                _handler.Handle(command, CancellationToken.None));
            
            await _saleRepository.Received(1).GetByIdAsync(saleId);
            await _saleRepository.DidNotReceive().UpdateAsync(Arg.Any<Sale>());
            await _unitOfWork.DidNotReceive().SaveChangesAsync();
        }

        [Fact]
        public async Task Handle_WithCancelledSale_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var saleId = Guid.NewGuid();
            var itemId = Guid.NewGuid();
            var command = new CancelSaleItemCommand(saleId, itemId);
            var sale = CancelSaleItemHandlerTestData.GetCancelledSaleWithItems();
            
            // Ensuring the test item exists in the sale
            sale.Items.First().Id = itemId;

            _saleRepository.GetByIdAsync(saleId).Returns(sale);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => 
                _handler.Handle(command, CancellationToken.None));
            
            await _saleRepository.Received(1).GetByIdAsync(saleId);
            await _saleRepository.DidNotReceive().UpdateAsync(Arg.Any<Sale>());
            await _unitOfWork.DidNotReceive().SaveChangesAsync();
        }
    }
}
