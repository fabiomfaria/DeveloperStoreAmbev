using System.ComponentModel.DataAnnotations;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSaleItem
{
    public class CancelSaleItemRequest
    {
        [Required]
        public Guid SaleId { get; set; }

        [Required]
        public Guid SaleItemId { get; set; }

        public string? CancellationReason { get; set; }
    }
}
