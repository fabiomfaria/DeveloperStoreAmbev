using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands.UpdateSale
{
    public class UpdateSaleValidator : AbstractValidator<UpdateSaleCommand>
    {
        public UpdateSaleValidator()
        {
            RuleFor(v => v.Id)
                .NotEmpty().WithMessage("The sale ID is required.");

            RuleFor(v => v.Items)
                .NotEmpty().WithMessage("The sale must have at least one item.");

            RuleForEach(v => v.Items).ChildRules(item =>
            {
                item.RuleFor(i => i.ProductId)
                    .NotEmpty().WithMessage("The product ID is required.");

                item.RuleFor(i => i.Quantity)
                    .GreaterThan(0).WithMessage("The quantity must be greater than zero.")
                    .LessThanOrEqualTo(20).WithMessage("You cannot sell more than 20 identical items.");

                item.RuleFor(i => i.UnitPrice)
                    .GreaterThan(0).WithMessage("The unit price must be greater than zero.");
            });
        }
    }
}
