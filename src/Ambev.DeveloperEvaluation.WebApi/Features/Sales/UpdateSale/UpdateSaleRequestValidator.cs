using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale
{
    public class UpdateSaleRequestValidator : AbstractValidator<UpdateSaleRequest>
    {
        public UpdateSaleRequestValidator()
        {
            RuleFor(x => x.SaleId)
                .NotEmpty().WithMessage("Sale ID is required");
                
            RuleFor(x => x.CustomerId)
                .NotEmpty().When(x => x.CustomerId.HasValue)
                .WithMessage("Customer ID must be valid if provided");
                
            RuleFor(x => x.BranchId)
                .NotEmpty().When(x => x.BranchId.HasValue)
                .WithMessage("Branch ID must be valid if provided");
                
            RuleForEach(x => x.Items)
                .SetValidator(new UpdateSaleItemRequestValidator())
                .When(x => x.Items != null && x.Items.Any());
        }
    }
    
    public class UpdateSaleItemRequestValidator : AbstractValidator<UpdateSaleItemRequest>
    {
        public UpdateSaleItemRequestValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("Product ID is required");
                
            RuleFor(x => x.Quantity)
                .NotEmpty().WithMessage("Quantity is required")
                .GreaterThan(0).WithMessage("Quantity must be greater than 0")
                .LessThanOrEqualTo(20).WithMessage("Quantity must not exceed 20 items");
                
            RuleFor(x => x.UnitPrice)
                .GreaterThan(0).When(x => x.UnitPrice.HasValue)
                .WithMessage("Unit price must be greater than 0 if provided");
        }
    }
}
