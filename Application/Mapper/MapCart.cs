using Application.DTOs.CartDTOs;
using Domain.Entities;
using Mapster;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Mapper
{
    public static class MapCart
    {
        public static void Configure()
        {
            TypeAdapterConfig<CartItem, CartItemDto>
                .NewConfig()
                .Map(dest => dest.ProductName, src => src.Product.ProductName)
                .Map(dest => dest.Price, src => src.Product.Price)
                .Map(dest => dest.Quantity, src => src.Quantity);

            TypeAdapterConfig<Cart, CartDto>
                .NewConfig()
                .Map(dest => dest.UserId, src => src.UserId)
                .Map(dest => dest.Items, src => src.CartItems.Adapt<List<CartItemDto>>());
        }
    }
}
