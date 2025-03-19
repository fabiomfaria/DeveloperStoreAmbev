using FluentValidation;
using Ambev.DeveloperEvaluation.Application.Sales.Commands.CancelSaleItem;

namespace Ambev.DeveloperEvaluation.Domain.Validators
{
    public class CancelSaleItemValidator : AbstractValidator<CancelSaleItemCommand>
    {
        public CancelSaleItemValidator()
        {
            RuleFor(x => x.SaleId)
                .NotEmpty().WithMessage("Sale ID is required.")
                .NotEqual(Guid.Empty).WithMessage("The sale ID cannot be an empty GUID.");

            RuleFor(x => x.SaleItemId)
                .NotEmpty().WithMessage("Item ID is required.")
                .NotEqual(Guid.Empty).WithMessage("The item ID cannot be an empty GUID.");
        }
    }
}