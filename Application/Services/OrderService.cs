using Application.DTOs.OrderDTOs;
using Application.Interfaces.Repository.Order_Repo;
using Application.Interfaces.Repository.Product_Repo;
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
        private readonly IProductRepository _productRepository;


        public OrderService(IOrderRepository orderRepository,
                             IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;

        }
        public async Task<OrderDto> CreateOrderAsync(CreateOrderDto dto)
        {
            if (dto.Items == null || !dto.Items.Any())
                throw new Exception("Order must contain at least one item.");

            var order = new Order
            {
                UserId = dto.UserId,
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Processing
            };
            decimal totalAmount = 0;

            foreach (var item in dto.Items)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product == null)
                    throw new Exception($"Product with Id {item.ProductId} not found");
                if (product.StockQuantity < item.Quantity)
                    throw new Exception($"Not enough Stock for product {product.Name}");
                var subTotal = product.Price * item.Quantity;
                var orderItem = new OrderItem
                {
                    ProductId = product.Id,
                    Quantity = item.Quantity,
                    Price = product.Price,
                    SubTotal = subTotal
                };
                product.StockQuantity -= item.Quantity;
                order.OrderItems.Add(orderItem);
                totalAmount += subTotal;
            }
            order.TotalAmount = totalAmount;
            await _orderRepository.AddAsync(order);
            await _orderRepository.SaveChangesAsync();

            return new OrderDto
            {
                Id = order.Id,
                UserId = order.UserId,
                OrderDate = order.OrderDate,
                Status = order.Status.ToString(),
                TotalAmount = order.TotalAmount,
                Items = order.OrderItems.Select(i => new OrderItemDto
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    Price = i.Price,
                    SubTotal = i.SubTotal

                }).ToList()
            };

        }


        async Task<OrderDto?> IOrderService.GetOrderByIdAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null) return null;

            return MapToDto(order);
        }

        async Task<OrderDto?> IOrderService.GetOrderWithItemsAsync(int id)
        {
            var order = await _orderRepository.GetByIdWithItemsAsync(id);
            if (order == null) return null;

            return MapToDto(order);
        }

        async Task<List<OrderDto>> IOrderService.GetOrdersByUserAsync(int userId)
        {
            var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);
            return orders.Select(MapToDto).ToList();
        }

        async Task<List<OrderDto>> IOrderService.GetOrdersByStatusAsync(OrderStatus status)
        {
            var orders = await _orderRepository.GetOrdersByStatusAsync(status);
            return orders.Select(MapToDto).ToList();
        }

        private OrderDto MapToDto(Order order)
        {
            return new OrderDto
            {
                Id = order.Id,
                UserId = order.UserId,
                OrderDate = order.OrderDate,
                Status = order.Status.ToString(),
                TotalAmount = order.TotalAmount,
                Items = order.OrderItems?.Select(i => new OrderItemDto
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    Price = i.Price,
                    SubTotal = i.SubTotal
                }).ToList() ?? new List<OrderItemDto>()
            };
        }

    }
}