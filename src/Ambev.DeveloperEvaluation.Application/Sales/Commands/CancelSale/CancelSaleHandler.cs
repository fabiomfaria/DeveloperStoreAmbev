using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Events;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands.CancelSale
{
    public class CancelSaleHandler : IRequestHandler<CancelSaleCommand, CancelSaleResult>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMediator _mediator;

        public CancelSaleHandler(ISaleRepository saleRepository, IMediator mediator)
        {
            _saleRepository = saleRepository ?? throw new ArgumentNullException(nameof(saleRepository));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<CancelSaleResult> Handle(CancelSaleCommand request, CancellationToken cancellationToken)
        {
            var sale = await _saleRepository.GetByIdAsync(request.Id);
            if (sale == null)
            {
                throw new ArgumentException("Sale not found.");
            }

            sale.Cancel();
            await _saleRepository.UpdateAsync(sale);

            var cancelEvent = new SaleCancelledEvent(sale.Id, sale.SaleNumber);
            await _mediator.Publish(cancelEvent, cancellationToken);

            return new CancelSaleResult
            {
                Id = sale.Id,
                SaleNumber = sale.SaleNumber,
                IsCancelled = sale.IsCancelled,
                //CancellationDate = sale.CancellationDate
            };
        }
    }
}
