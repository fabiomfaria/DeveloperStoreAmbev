using System.ComponentModel.DataAnnotations;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale
{
    public class CancelSaleRequest
    {
        [Required]
        public Guid SaleId { get; set; }

        public string? CancellationReason { get; set; }
    }
}
