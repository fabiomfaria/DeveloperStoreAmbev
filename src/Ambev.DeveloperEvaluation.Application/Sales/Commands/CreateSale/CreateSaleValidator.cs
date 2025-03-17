using FluentValidation;
using System;
using System.Linq;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands.CreateSale
{
    public class CreateSaleValidator : AbstractValidator<CreateSaleCommand>
    {
        public CreateSaleValidator()
        {
            RuleFor(v => v.CustomerId)
                .NotEmpty().WithMessage("Customer ID is required.");

            RuleFor(v => v.BranchId)
                .NotEmpty().WithMessage("Branch ID is required.");

            RuleFor(v => v.Date)
                .NotEmpty().WithMessage("The date of sale is mandatory.")
                .Must(date => date <= DateTime.Now).WithMessage("The sale date cannot be in the future.");

            RuleFor(v => v.Items)
                .NotEmpty().WithMessage("The sale must have at least one item.");

            RuleForEach(v => v.Items).ChildRules(item =>
            {
                item.RuleFor(i => i.ProductId)
                    .NotEmpty().WithMessage("Product ID is required.");

                item.RuleFor(i => i.Quantity)
                    .GreaterThan(0).WithMessage("The quantity must be greater than zero.")
                    .LessThanOrEqualTo(20).WithMessage("You cannot sell more than 20 identical items.");

                item.RuleFor(i => i.UnitPrice)
                    .GreaterThan(0).WithMessage("The unit price must be greater than zero.");
            });
        }
    }
}