using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Application.Commands;
using Ambev.DeveloperEvaluation.Application.Commands.Handlers;
using Ambev.DeveloperEvaluation.Application.Dtos;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Interfaces;
using Ambev.DeveloperEvaluation.Domain.Validation;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application
{
    public class UpdateSaleHandlerTests
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISaleValidator _saleValidator;
        private readonly IEventPublisher _eventPublisher;
        private readonly UpdateSaleHandler _handler;

        public UpdateSaleHandlerTests()
        {
            _saleRepository = Substitute.For<ISaleRepository>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _mapper = Substitute.For<IMapper>();
            _saleValidator = Substitute.For<ISaleValidator>();
            _eventPublisher = Substitute.For<IEventPublisher>();
            _handler = new UpdateSaleHandler(
                _saleRepository,
                _unitOfWork,
                _mapper,
                _saleValidator,
                _eventPublisher);
        }

        [Fact]
        public async Task Handle_WithValidCommand_ShouldUpdateSaleAndPublishEvent()
        {
            // Arrange
            var command = UpdateSaleHandlerTestData.GetValidUpdateSaleCommand();
            var existingSale = UpdateSaleHandlerTestData.GetExistingSale();
            var updatedSale = UpdateSaleHandlerTestData.GetUpdatedSale();
            var expectedResponse = UpdateSaleHandlerTestData.GetUpdateSaleResponse();

            _saleRepository.GetByIdAsync(command.Id).Returns(existingSale);
            _saleRepository.UpdateAsync(Arg.Any<Sale>()).Returns(updatedSale);
            _mapper.Map<UpdateSaleResponse>(updatedSale).Returns(expectedResponse);
            _saleValidator.Validate(Arg.Any<Sale>()).Returns(new ValidationResult());

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResponse.Id, result.Id);
            
            await _saleRepository.Received(1).GetByIdAsync(command.Id);
            await _saleRepository.Received(1).UpdateAsync(Arg.Any<Sale>());
            await _unitOfWork.Received(1).SaveChangesAsync();
            await _eventPublisher.Received(1).PublishAsync(Arg.Any<string>(), Arg.Any<object>());
        }

        [Fact]
        public async Task Handle_WithNonExistingSale_ShouldThrowException()
        {
            // Arrange
            var command = UpdateSaleHandlerTestData.GetValidUpdateSaleCommand();
            _saleRepository.GetByIdAsync(command.Id).Returns((Sale)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => 
                _handler.Handle(command, CancellationToken.None));
            
            await _saleRepository.Received(1).GetByIdAsync(command.Id);
            await _saleRepository.DidNotReceive().UpdateAsync(Arg.Any<Sale>());
            await _unitOfWork.DidNotReceive().SaveChangesAsync();
        }

        [Fact]
        public async Task Handle_WithInvalidSale_ShouldThrowValidationException()
        {
            // Arrange
            var command = UpdateSaleHandlerTestData.GetValidUpdateSaleCommand();
            var existingSale = UpdateSaleHandlerTestData.GetExistingSale();
            
            _saleRepository.GetByIdAsync(command.Id).Returns(existingSale);
            
            var validationResult = new ValidationResult();
            validationResult.AddError("Test error");
            _saleValidator.Validate(Arg.Any<Sale>()).Returns(validationResult);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => 
                _handler.Handle(command, CancellationToken.None));
            
            Assert.Contains("Test error", exception.Message);
            
            await _saleRepository.Received(1).GetByIdAsync(command.Id);
            await _saleRepository.DidNotReceive().UpdateAsync(Arg.Any<Sale>());
            await _unitOfWork.DidNotReceive().SaveChangesAsync();
        }
    }
}
