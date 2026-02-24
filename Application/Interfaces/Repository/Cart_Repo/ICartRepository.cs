using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Repository.Cart_Repo
{
    public interface ICartRepository
    {
        Task<Cart> GetByIdAsync(int id);
        Task<Cart> GetCartByUserIdAsync(int userId);
        Task AddAsync(Cart cart);
        void Update(Cart cart);
        void Delete(Cart cart);
    }
}
