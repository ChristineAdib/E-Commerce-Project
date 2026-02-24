using Application.Interfaces.Repository.Cart_Repo;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class CartItemRepository : ICartItemRepository
    {
        private readonly ApplicationDbContext _context;

        public CartItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CartItem> GetByIdAsync(int id)
            => await _context.CartItems.FindAsync(id);

        public async Task<CartItem> GetByCartAndProductAsync(int cartId, int productId)
            => await _context.CartItems
                .FirstOrDefaultAsync(ci =>
                    ci.CartId == cartId &&
                    ci.ProductId == productId);

        public async Task AddAsync(CartItem item)
        {
            await _context.CartItems.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public void Update(CartItem item)
        {
            _context.CartItems.Update(item);
            _context.SaveChanges();
        }

        public void Delete(CartItem item)
        {
            _context.CartItems.Remove(item);
            _context.SaveChanges();
        }
    }
}
