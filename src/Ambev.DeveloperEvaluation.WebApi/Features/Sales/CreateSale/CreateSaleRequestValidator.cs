using FluentValidation;
using System;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale
{
    public class CreateSaleRequestValidator : AbstractValidator<CreateSaleRequest>
    {
        public CreateSaleRequestValidator()
        {
            RuleFor(x => x.SaleDate)
                .NotEmpty().WithMessage("Sale date is required")
                .Must(date => date.Date <= DateTime.Now.Date).WithMessage("Sale date cannot be in the future");

            RuleFor(x => x.CustomerId)
                .NotEmpty().WithMessage("Customer ID is required");

            RuleFor(x => x.CustomerName)
                .NotEmpty().WithMessage("Customer name is required")
                .MaximumLength(100).WithMessage("Customer name cannot exceed 100 characters");

            RuleFor(x => x.BranchId)
                .NotEmpty().WithMessage("Branch ID is required");

            RuleFor(x => x.BranchName)
                .NotEmpty().WithMessage("Branch name is required")
                .MaximumLength(100).WithMessage("Branch name cannot exceed 100 characters");

            RuleFor(x => x.Items)
                .NotEmpty().WithMessage("At least one item is required");

            RuleForEach(x => x.Items).SetValidator(new CreateSaleItemRequestValidator());
        }
    }

    public class CreateSaleItemRequestValidator : AbstractValidator<CreateSaleItemRequest>
    {
        public CreateSaleItemRequestValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("Product ID is required");

            RuleFor(x => x.ProductName)
                .NotEmpty().WithMessage("Product name is required")
                .MaximumLength(100).WithMessage("Product name cannot exceed 100 characters");

            RuleFor(x => x.Quantity)
                .NotEmpty().WithMessage("Quantity is required")
                .GreaterThan(0).WithMessage("Quantity must be greater than 0")
                .LessThanOrEqualTo(20).WithMessage("Quantity cannot exceed 20");

            RuleFor(x => x.UnitPrice)
                .NotEmpty().WithMessage("Unit price is required")
                .GreaterThan(0).WithMessage("Unit price must be greater than 0");
        }
    }
}