using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Repository.Cart_Repo
{
    public interface ICartItemRepository
    {
        Task<CartItem> GetByIdAsync(int id);
        Task<CartItem> GetByCartAndProductAsync(int cartId, int productId);
        Task AddAsync(CartItem item);
        void Update(CartItem item);
        void Delete(CartItem item);
    }
}
