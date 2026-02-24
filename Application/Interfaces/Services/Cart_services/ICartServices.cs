using Application.DTOs.CartDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Services.Cart_services
{
    public interface ICartServices
    {

        Task<CartDto> GetCartAsync(int userId);
        Task AddToCartAsync(int userId, int productId, int quantity);
        Task RemoveFromCartAsync(int userId, int productId);
        public Task UpdateQuantityAsync(int userId, int productId, int quantity);
        Task<decimal> GetCartTotalAsync(int userId);
        public Task ClearCartAsync(int userId);
    }
}
