using Ambev.DeveloperEvaluation.Common.Validation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.Queries.GetSaleById
{
    public class GetSaleByIdValidator : Validator<GetSaleByIdQuery>
    {
        public override Task<ValidationResult> ValidateAsync(GetSaleByIdQuery instance)
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

            return Task.FromResult(new ValidationResult
            {
                IsValid = errors.Count == 0,
                Errors = errors
            });
        }
    }
}
