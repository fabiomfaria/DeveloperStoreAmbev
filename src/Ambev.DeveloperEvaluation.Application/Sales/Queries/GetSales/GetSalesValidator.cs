using FluentValidation;
using Ambev.DeveloperEvaluation.Application.Sales.Queries.GetSales;

namespace Ambev.DeveloperEvaluation.Application.Sales.Queries.GetSales
{
    public class GetSalesValidator : AbstractValidator<GetSalesQuery>
    {
        public GetSalesValidator()
        {
            RuleFor(x => x.PageSize)
                .LessThanOrEqualTo(100).WithMessage("Page size cannot exceed 100.");

            RuleFor(x => x)
                .Must(x => !x.StartDate.HasValue || !x.EndDate.HasValue || x.StartDate <= x.EndDate)
                .WithMessage("The start date cannot be later than the end date.");
        }
    }
}
