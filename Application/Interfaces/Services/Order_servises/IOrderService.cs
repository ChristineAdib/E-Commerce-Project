using Application.DTOs.OrderDTOs;
using Application.Interfaces.Repository.Order_Repo;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Services.Order_servises
{
    public interface IOrderService
    {
        Task<OrderDto?> GetOrderByIdAsync(int id);
        Task<OrderDto?> GetOrderWithItemsAsync(int id);
        Task<List<OrderDto>> GetOrdersByUserAsync(int userId);
        Task<List<OrderDto>> GetOrdersByStatusAsync(OrderStatus status);
        Task<OrderDto> CreateOrderAsync(CreateOrderDto dto);
    }


}
    
}
