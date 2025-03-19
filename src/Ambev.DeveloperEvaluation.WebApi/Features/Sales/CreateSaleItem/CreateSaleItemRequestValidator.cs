using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSaleItem
{
    public class CreateSaleItemRequestValidator : AbstractValidator<CreateSaleItemRequest>
    {
        public CreateSaleItemRequestValidator()
        {
            RuleFor(request => request.ProductId)
                .NotEmpty().WithMessage("Product ID is required.");

            RuleFor(request => request.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.")
                .LessThanOrEqualTo(20).WithMessage("Cannot sell more than 20 identical items.");

            RuleFor(request => request.UnitPrice)
                .GreaterThan(0).WithMessage("Unit price must be greater than zero.");

            RuleFor(request => request.DiscountPercentage)
                .Must((request, discountPercentage) => IsValidDiscount(request.Quantity, discountPercentage))
                .WithMessage("Invalid discount percentage for the given quantity.");
        }

        private bool IsValidDiscount(int quantity, decimal? discountPercentage)
        {
            // No discount allowed for quantities less than 4
            if (quantity < 4 && discountPercentage.HasValue && discountPercentage.Value > 0)
                return false;

            // 10% discount for quantities between 4 and 9
            if (quantity >= 4 && quantity < 10)
                return !discountPercentage.HasValue || discountPercentage.Value <= 10;

            // 20% discount for quantities between 10 and 20
            if (quantity >= 10 && quantity <= 20)
                return !discountPercentage.HasValue || discountPercentage.Value <= 20;

            return true;
        }
    }
}
