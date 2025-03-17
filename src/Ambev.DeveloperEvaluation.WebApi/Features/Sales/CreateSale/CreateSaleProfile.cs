using Ambev.DeveloperEvaluation.Application.Sales.Commands.CreateSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale
{
    public class CreateSaleProfile : Profile
    {
        public CreateSaleProfile()
        {
            // Map from WebApi request to Application command
            CreateMap<CreateSaleRequest, CreateSaleCommand>();
            CreateMap<CreateSaleItemRequest, CreateSaleItemCommand>();

            // Map from Application result to WebApi response
            CreateMap<CreateSaleResult, CreateSaleResponse>();
            CreateMap<CreateSaleItemResult, CreateSaleItemResponse>();
        }
    }
}