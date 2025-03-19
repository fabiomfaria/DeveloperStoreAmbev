using Ambev.DeveloperEvaluation.Application.Sales.Commands.CancelSale;
using Ambev.DeveloperEvaluation.Application.Sales.Commands.CancelSaleItem;
using Ambev.DeveloperEvaluation.Application.Sales.Commands.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.Commands.UpdateSale;
using Ambev.DeveloperEvaluation.Application.Sales.Queries.GetSaleById;
using Ambev.DeveloperEvaluation.Application.Sales.Queries.GetSales;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSaleItem;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSaleById;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSales;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Ambev.DeveloperEvaluation.Application.Sales.Commands.CreateSaleItem;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public SalesController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<IActionResult> GetSales([FromQuery] GetSalesRequest request, CancellationToken cancellationToken)
        {
            var query = _mapper.Map<GetSalesQuery>(request);
            var result = await _mediator.Send(query, cancellationToken);

            var response = _mapper.Map<GetSalesResponse>(result);
            return Ok(new ApiResponseWithData<GetSalesResponse> { Data = response });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSaleById(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetSaleByIdQuery { SaleId = id };
            var result = await _mediator.Send(query, cancellationToken);

            var response = _mapper.Map<GetSaleByIdResponse>(result);
            return Ok(new ApiResponseWithData<GetSaleByIdResponse> { Data = response });
        }

        [HttpPost]
        public async Task<IActionResult> CreateSale([FromBody] CreateSaleRequest request, CancellationToken cancellationToken)
        {
            var command = _mapper.Map<CreateSaleCommand>(request);
            var result = await _mediator.Send(command, cancellationToken);

            var response = _mapper.Map<CreateSaleResponse>(result);
            return CreatedAtAction(nameof(GetSaleById), new { id = response.SaleId }, new ApiResponseWithData<CreateSaleResponse> { Data = response });
        }

        [HttpPost("{saleId}/items")]
        public async Task<IActionResult> CreateSaleItem(Guid saleId, [FromBody] CreateSaleItemRequest request, CancellationToken cancellationToken)
        {
            var command = _mapper.Map<CreateSaleItemCommand>(request);
            command.SaleId = saleId;

            var result = await _mediator.Send(command, cancellationToken);

            var response = _mapper.Map<CreateSaleItemResponse>(result);
            return CreatedAtAction(nameof(GetSaleById), new { id = saleId }, new ApiResponseWithData<CreateSaleItemResponse> { Data = response });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSale(Guid id, [FromBody] UpdateSaleRequest request, CancellationToken cancellationToken)
        {
            if (id != request.SaleId)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Sale ID in URL does not match the one in request body."
                });
            }

            var command = _mapper.Map<UpdateSaleCommand>(request);
            var result = await _mediator.Send(command, cancellationToken);

            var response = _mapper.Map<UpdateSaleResponse>(result);
            return Ok(new ApiResponseWithData<UpdateSaleResponse> { Data = response });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelSale(Guid id, CancellationToken cancellationToken)
        {
            var command = new CancelSaleCommand { Id = id };
            var result = await _mediator.Send(command, cancellationToken);

            var response = _mapper.Map<CancelSaleResponse>(result);
            return Ok(new ApiResponseWithData<CancelSaleResponse> { Data = response });
        }

        [HttpDelete("{saleId}/items/{itemId}")]
        public async Task<IActionResult> CancelSaleItem(Guid saleId, Guid itemId, CancellationToken cancellationToken)
        {
            var command = new CancelSaleItemCommand { SaleId = saleId, SaleItemId = itemId };
            var result = await _mediator.Send(command, cancellationToken);

            var response = _mapper.Map<CancelSaleItemResponse>(result);
            return Ok(new ApiResponseWithData<CancelSaleItemResponse> { Data = response });
        }
    }
}