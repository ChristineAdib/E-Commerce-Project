using Application.Interfaces.Repository.Order_Repo;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Services.Order_servises
{
    public interface IOrderService
    {
        Task<Order?> GetOrderByIdAsync(int id);
        Task<Order?> GetOrderWithItemsAsync(int id);
        Task<List<Order>> GetOrdersByUserAsync(int userId);
        Task<List<Order>> GetOrdersByStatusAsync(OrderStatus status);
        Task CreateOrderAsync(Order order);


    }
    
}
