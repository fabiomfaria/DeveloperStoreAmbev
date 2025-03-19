using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands.CreateSaleItem
{
    public class CreateSaleItemProfile : Profile
    {
        public CreateSaleItemProfile()
        {
            CreateMap<Domain.Entities.SaleItem, CreateSaleItemResult>()
                .ForMember(dest => dest.ProductName, opt => opt.Ignore());
        }
    }
}
