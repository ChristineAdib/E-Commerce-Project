using Application.Interfaces.Repository.Cart_Repo;
using Application.Interfaces.Repository.Ctegory_Repo;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class CartRepository : ICartRepository
    {
        public readonly ApplicationDbContext _context;
        public CartRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Cart cart)
        {
            await _context.AddAsync(cart);
            await _context.SaveChangesAsync();
        }

        public void Delete(Cart cart)
        {
            _context.Remove(cart);
            _context.SaveChanges();
        }

        public async Task<Cart> GetByIdAsync(int id)
        {
            return await _context.Carts.FindAsync(id);
        }

        public async Task<Cart> GetCartByUserIdAsync(int userId)
        {
            return await _context.Carts.Include(c => c.CartItems)
                                        .ThenInclude(i => i.Product)
                                        .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public void Update(Cart cart)
        {
            _context.Update(cart);
            _context.SaveChanges();
        }
    }
}
