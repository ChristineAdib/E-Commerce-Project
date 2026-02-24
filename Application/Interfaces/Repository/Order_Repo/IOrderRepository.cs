using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Repository.Order_Repo
{
    public interface IOrderRepository
    {
        Task AddAsync(Order order);
        Task SaveChangesAsync();
        Task<Order?> GetByIdAsync(int id);
        Task<Order?> GetByIdWithItemsAsync(int id);
        Task<List<Order>> GetOrdersByUserIdAsync(int userId);
        Task<List<Order>> GetOrdersByStatusAsync(OrderStatus Status);
    }
}
