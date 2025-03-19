using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands.CancelSale
{
    public class CancelSaleProfile : Profile
    {
        public CancelSaleProfile()
        {
            CreateMap<Sale, CancelSaleResult>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.SaleNumber, opt => opt.MapFrom(src => src.SaleNumber))
                .ForMember(dest => dest.IsCancelled, opt => opt.MapFrom(src => src.IsCancelled))
                .ForMember(dest => dest.CancellationDate, opt => opt.MapFrom(src => src.Date));
        }
    }
}
