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
    public class GetSalesHandlerTests
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;
        private readonly GetSalesHandler _handler;

        public GetSalesHandlerTests()
        {
            _saleRepository = Substitute.For<ISaleRepository>();
            _mapper = Substitute.For<IMapper>();
            _handler = new GetSalesHandler(_saleRepository, _mapper);
        }

        [Fact]
        public async Task Handle_WithValidQuery_ShouldReturnPagedResult()
        {
            // Arrange
            var query = GetSalesHandlerTestData.GetValidGetSalesQuery();
            var sales = GetSalesHandlerTestData.GetSalesList();
            var expectedResponses = GetSalesHandlerTestData.GetSalesResponseList();
            var pagedResult = new PagedResult<Sale>(sales, 10, query.Page, query.PageSize);

            _saleRepository.GetPagedAsync(
                query.Page, 
                query.PageSize, 
                query.SortBy, 
                query.SortDirection,
                query.CustomerName,
                query.BranchName,
                query.MinDate,
                query.MaxDate).Returns(pagedResult);

            _mapper.Map<IEnumerable<SaleListItemResponse>>(sales).Returns(expectedResponses);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(pagedResult.TotalItems, result.TotalItems);
            Assert.Equal(pagedResult.Page, result.Page);
            Assert.Equal(pagedResult.PageSize, result.PageSize);
            Assert.Equal(pagedResult.TotalPages, result.TotalPages);
            
            await _saleRepository.Received(1).GetPagedAsync(
                query.Page, 
                query.PageSize, 
                query.SortBy, 
                query.SortDirection,
                query.CustomerName,
                query.BranchName,
                query.MinDate,
                query.MaxDate);
        }

        [Fact]
        public async Task Handle_WithEmptyResult_ShouldReturnEmptyPagedResult()
        {
            // Arrange
            var query = GetSalesHandlerTestData.GetValidGetSalesQuery();
            var sales = new List<Sale>();
            var expectedResponses = new List<SaleListItemResponse>();
            var pagedResult = new PagedResult<Sale>(sales, 0, query.Page, query.PageSize);

            _saleRepository.GetPagedAsync(
                query.Page, 
                query.PageSize, 
                query.SortBy, 
                query.SortDirection,
                query.CustomerName,
                query.BranchName,
                query.MinDate,
                query.MaxDate).Returns(pagedResult);

            _mapper.Map<IEnumerable<SaleListItemResponse>>(sales).Returns(expectedResponses);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.TotalItems);
            Assert.Equal(query.Page, result.Page);
            Assert.Equal(query.PageSize, result.PageSize);
            Assert.Equal(0, result.TotalPages);
            Assert.Empty(result.Items);
        }
    }
}
