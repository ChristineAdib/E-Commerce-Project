using Application.Interfaces.Repository.Order_Repo;
using Application.Interfaces.Services.Order_servises;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await _orderRepository.GetByIdAsync(id);
        }

        public async Task<Order?> GetOrderWithItemsAsync(int id)
        {
            return await _orderRepository.GetByIdWithItemsAsync(id);
        }

        public async Task<List<Order>> GetOrdersByUserAsync(int userId)
        {
            return await _orderRepository.GetOrdersByUserIdAsync(userId);
        }

        public async Task<List<Order>> GetOrdersByStatusAsync(OrderStatus status)
        {
            return await _orderRepository.GetOrdersByStatusAsync(status);
        }

        public async Task CreateOrderAsync(Order order)
        {
            order.Status = OrderStatus.Processing;
            order.OrderDate = DateTime.UtcNow;

            await _orderRepository.AddAsync(order);
            await _orderRepository.SaveChangesAsync();
        }
    }
}