using Ambev.DeveloperEvaluation.Common.Validation;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.Queries.GetSales
{
    public class GetSalesValidator : Validator<GetSalesQuery>
    {
        public override Task<ValidationResult> ValidateAsync(GetSalesQuery instance)
        {
            var errors = new List<ValidationErrorDetail>();

            if (instance.PageSize > 100)
            {
                errors.Add(new ValidationErrorDetail
                {
                    Field = nameof(instance.PageSize),
                    Message = "Page size cannot exceed 100."
                });
            }

            if (instance.StartDate.HasValue && instance.EndDate.HasValue && instance.StartDate > instance.EndDate)
            {
                errors.Add(new ValidationErrorDetail
                {
                    Field = nameof(instance.StartDate),
                    Message = "Start date cannot be after end date."
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
