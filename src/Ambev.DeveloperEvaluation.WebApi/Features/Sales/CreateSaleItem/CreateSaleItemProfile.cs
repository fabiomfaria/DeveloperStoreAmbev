using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.Commands.CreateSaleItem;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSaleItem
{
    public class CreateSaleItemProfile : Profile
    {
        public CreateSaleItemProfile()
        {
            CreateMap<CreateSaleItemRequest, CreateSaleItemCommand>();
            CreateMap<CreateSaleItemResult, CreateSaleItemResponse>();
        }
    }
}
