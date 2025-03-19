using FluentValidation;
using Ambev.DeveloperEvaluation.Application.Sales.Queries.GetSaleById;

namespace Ambev.DeveloperEvaluation.Application.Sales.Queries.GetSaleById
{
    public class GetSaleByIdValidator : AbstractValidator<GetSaleByIdQuery>
    {
        public GetSaleByIdValidator()
        {
            RuleFor(x => x.SaleId)
                .NotEmpty().WithMessage("Sale ID is required.")
                .NotEqual(Guid.Empty).WithMessage("The sale ID cannot be an empty GUID.");
        }
    }
}
