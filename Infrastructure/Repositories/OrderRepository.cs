using Application.Interfaces.Repository.Order_Repo;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

       public async Task AddAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _context.Orders.
                                        AsNoTracking()
                                       .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<Order?> GetByIdWithItemsAsync(int id)
        {
            return await _context.Orders
                                  
                                  .Include(o => o.OrderItems)
                                  .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<List<Order>> GetOrdersByStatusAsync(OrderStatus Status)
        {
            return await _context.Orders
                                  .AsNoTracking()
                                  .Where(o => o.Status == Status)
                                  .ToListAsync();
        }

        public async Task<List<Order>> GetOrdersByUserIdAsync(int userId)
        {
            return await _context.Orders
                                 .AsNoTracking()
                                 .Where(o => o.UserId == userId)
                                 .ToListAsync();
        }
    }
}
