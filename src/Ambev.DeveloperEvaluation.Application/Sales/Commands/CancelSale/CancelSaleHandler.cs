using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands.CancelSale
{
    public class CancelSaleHandler : IRequestHandler<CancelSaleCommand, CancelSaleResult>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public CancelSaleHandler(ISaleRepository saleRepository, IMapper mapper, IMediator mediator)
        {
            _saleRepository = saleRepository ?? throw new ArgumentNullException(nameof(saleRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<CancelSaleResult> Handle(CancelSaleCommand request, CancellationToken cancellationToken)
        {
            // Retrieve the sale
            var sale = await _saleRepository.GetByIdAsync(request.SaleId, cancellationToken);

            if (sale == null)
            {
                throw new DomainException($"Sale with ID {request.SaleId} not found.");
            }

            // Check if the sale is already cancelled
            if (sale.Cancelled)
            {
                throw new DomainException($"Sale with ID {request.SaleId} is already cancelled.");
            }

            // Cancel the sale
            sale.Cancel();

            // Update the sale in the repository
            await _saleRepository.UpdateAsync(sale, cancellationToken);

            // Publish the SaleCancelled event
            await _mediator.Publish(new SaleCancelledEvent { SaleId = sale.Id }, cancellationToken);

            // Map and return the result
            return _mapper.Map<CancelSaleResult>(sale);
        }
    }
}