using Ambev.DeveloperEvaluation.Application.Sales.Queries.GetSaleById;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSaleById
{
    public class GetSaleByIdProfile : Profile
    {
        public GetSaleByIdProfile()
        {
            // Map from Application result to WebApi response
            CreateMap<GetSaleByIdResult, GetSaleByIdResponse>();
            CreateMap<SaleItemDto, SaleItemResponse>();
        }
    }
}