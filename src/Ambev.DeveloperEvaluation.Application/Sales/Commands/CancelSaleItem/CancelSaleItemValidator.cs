using Ambev.DeveloperEvaluation.Common.Validation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands.CancelSaleItem
{
    public class CancelSaleItemValidator : Validator<CancelSaleItemCommand>
    {
        public override Task<ValidationResult> ValidateAsync(CancelSaleItemCommand instance)
        {
            var errors = new List<ValidationErrorDetail>();

            if (instance.SaleId == Guid.Empty)
            {
                errors.Add(new ValidationErrorDetail
                {
                    Field = nameof(instance.SaleId),
                    Message = "Sale ID cannot be empty."
                });
            }

            if (instance.SaleItemId == Guid.Empty)
            {
                errors.Add(new ValidationErrorDetail
                {
                    Field = nameof(instance.SaleItemId),
                    Message = "Sale Item ID cannot be empty."
                });
            }

            return Task.FromResult(new ValidationResult
            {
                IsValid = errors.Count == 0,
                Errors = errors
            });
        }
    }
}
