﻿using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.Commands.CancelSaleItem;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSaleItem
{
    public class CancelSaleItemProfile : Profile
    {
        public CancelSaleItemProfile()
        {
            CreateMap<CancelSaleItemRequest, CancelSaleItemCommand>();
            CreateMap<CancelSaleItemResult, CancelSaleItemResponse>();
        }
    }
}
