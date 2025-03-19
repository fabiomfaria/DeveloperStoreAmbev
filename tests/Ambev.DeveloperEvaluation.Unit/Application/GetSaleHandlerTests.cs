using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Application.Dtos;
using Ambev.DeveloperEvaluation.Application.Queries;
using Ambev.DeveloperEvaluation.Application.Queries.Handlers;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Interfaces;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application
{
    public class GetSaleHandlerTests
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;
        private readonly GetSaleHandler _handler;

        public GetSaleHandlerTests()
        {
            _saleRepository = Substitute.For<ISaleRepository>();
            _mapper = Substitute.For<IMapper>();
            _handler = new GetSaleHandler(_saleRepository, _mapper);
        }

        [Fact]
        public async Task Handle_WithExistingSaleId_ShouldReturnSaleResponse()
        {
            // Arrange
            var query = new GetSaleQuery(Guid.NewGuid());
            var sale = GetSaleHandlerTestData.GetExistingSale();
            var expectedResponse = GetSaleHandlerTestData.GetSaleResponse();

            _saleRepository.GetByIdAsync(query.Id).Returns(sale);
            _mapper.Map<SaleResponse>(sale).Returns(expectedResponse);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResponse.Id, result.Id);
            Assert.Equal(expectedResponse.SaleNumber, result.SaleNumber);
            Assert.Equal(expectedResponse.CustomerName, result.CustomerName);
            Assert.Equal(expectedResponse.TotalAmount, result.TotalAmount);
            
            await _saleRepository.Received(1).GetByIdAsync(query.Id);
        }

        [Fact]
        public async Task Handle_WithNonExistingSaleId_ShouldThrowNotFoundException()
        {
            // Arrange
            var query = new GetSaleQuery(Guid.NewGuid());
            _saleRepository.GetByIdAsync(query.Id).Returns((Sale)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => 
                _handler.Handle(query, CancellationToken.None));
            
            await _saleRepository.Received(1).GetByIdAsync(query.Id);
        }
    }
}
