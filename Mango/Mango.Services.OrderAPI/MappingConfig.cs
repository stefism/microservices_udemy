﻿using AutoMapper;
using Mango.Services.OrderAPI.Models;
using Mango.Services.OrderAPI.Models.Dto;

namespace Mango.Services.OrderAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<OrderHeaderDto, CartHeaderDto>()
                .ForMember(dest => dest.CartTotal, x => x.MapFrom(src => src.OrderTotal))
                .ReverseMap();

                config.CreateMap<CartDetailsDto, OrderDetailsDto>()
                .ForMember(dest => dest.ProductName, x => x.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.ProductPrice, x => x.MapFrom(src => src.Product.Price));

                config.CreateMap<OrderDetailsDto, CartDetailsDto>();
                
                config.CreateMap<OrderHeader, OrderHeaderDto>().ReverseMap();
                config.CreateMap<OrderDetailsDto, OrderDetails>().ReverseMap();
            });

            return mappingConfig;
        }
    }
}
