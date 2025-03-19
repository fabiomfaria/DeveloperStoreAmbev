using Ambev.DeveloperEvaluation.Application.Sales.Commands.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.Queries.GetSaleById
{
    public class GetSaleByIdProfile : Profile
    {
        public GetSaleByIdProfile()
        {
            CreateMap<Sale, GetSaleByIdResult>()
                .ForMember(dest => dest.SaleId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.SaleNumber, opt => opt.MapFrom(src => src.SaleNumber))
                .ForMember(dest => dest.SaleDate, opt => opt.MapFrom(src => src.Date))
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.Name)) 
                .ForMember(dest => dest.BranchId, opt => opt.MapFrom(src => src.BranchId))
                .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch.Name)) 
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
                .ForMember(dest => dest.Cancelled, opt => opt.MapFrom(src => src.IsCancelled))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

            CreateMap<SaleItem, SaleItemDto>()
                .ForMember(dest => dest.ItemId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name)) 
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice))
                .ForMember(dest => dest.DiscountPercentage, opt => opt.MapFrom(src => src.Discount))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
                .ForMember(dest => dest.Cancelled, opt => opt.MapFrom(src => src.IsCancelled));
        }
    }
}
