using Application.DTOs.OrderDTOs;
using Application.Interfaces.Repository.Cart_Repo;
using Application.Interfaces.Repository.Ctegory_Repo;
using Application.Interfaces.Repository.Product_Repo;
using Application.Interfaces.Repository.User_Repo;
using Application.Interfaces.Services.Cart_services;
using Application.Interfaces.Services.Category_servises;
using Application.Interfaces.Services.Order_servises;
using Application.Interfaces.Services.Product_services;
using Application.Interfaces.Services.User_services;
using Application.Mapper;
using Application.Services;
using Autofac;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extensions.DependencyInjection;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Presentation
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            

            using var context = new ApplicationDbContext();
            var productRepo = new ProductRepository(context);
            var orderRepo = new OrderRepository(context);

            IOrderService orderService = new OrderService(orderRepo, productRepo);


            //var category = new Category
            //{
            //    CategoryName = "Test Category",
            //    CategoryDescription = "Demo Description"
            //};

            //await context.Categories.AddAsync(category);
            //await context.SaveChangesAsync();


            //var product = new Product
            //{
            //    ProductName = "Test Product5",
            //    Price = 100,
            //    Description = "Demo",
            //    ImageUrl = "img.jpg",
            //    CategoryID = 2,
            //    StockQuantity = 10
            //};

            //await context.Products.AddAsync(product);
            //await context.SaveChangesAsync();

            //Console.WriteLine($"Before Order Stock: {product.StockQuantity}");

            //var orderDto = new CreateOrderDto
            //{
            //    UserId = 1,
            //    Items =
            //    {
            //        new CreateOrderItemDto
            //        {
            //            ProductId = product.Id,
            //            Quantity = 4
            //        }
            //    }

            //};

            //var createdOrder = await orderService.CreateOrderAsync(orderDto);

            //Console.WriteLine($"Product Added with Id: {product.Id}");
            //Console.WriteLine($"Order Created with Id: {createdOrder.Id}");
            //Console.WriteLine($"Total Amount: {createdOrder.TotalAmount}");

            //var updatedProduct = await context.Products
            //                                   .AsNoTracking()
            //                                   .FirstOrDefaultAsync(p => p.Id == product.Id);

            //Console.WriteLine($"After Order Stock From DB: {updatedProduct.StockQuantity}");

            //Console.ReadLine();


            //var userOrders = await orderService.GetOrdersByUserIdAsync(1);

            //    var userOrders = await orderService.GetOrdersByUserAsync(1);
            //    Console.WriteLine($"Orders Count For User 1: {userOrders.Count}");

            //    foreach (var order in userOrders)
            //    {
            //        Console.WriteLine("--------------------------------");
            //        Console.WriteLine($"Order Id: {order.Id}");
            //        Console.WriteLine($"Status: {order.Status}");
            //        Console.WriteLine($"Total: {order.TotalAmount}");
            //        Console.WriteLine("Items:");

            //        foreach (var item in order.Items)
            //        {
            //            Console.WriteLine($"  ProductId: {item.ProductId}");
            //            Console.WriteLine($"  Quantity: {item.Quantity}");
            //            Console.WriteLine($"  SubTotal: {item.SubTotal}");
            //            Console.WriteLine();
            //        }
            //    }

            //    Console.ReadLine();

            var orderById = await orderService.GetOrderByIdAsync(2);
            if (orderById != null)
            {
                Console.WriteLine($"Order Id: {orderById.Id}");
                Console.WriteLine($"Status: {orderById.Status}");
                Console.WriteLine($"Total: {orderById.TotalAmount}");
            }
            else
            {
                Console.WriteLine("Order not found");
            }


            var updatedOrder = await orderService
             .UpdateOrderStatusAsync(2, OrderStatus.Shipped);
            context.ChangeTracker.Clear();

            Console.WriteLine($"Updated Status: {updatedOrder.Status}");

            //await context.SaveChangesAsync();


            /////////////////////////////////////////////
            var processingOrders = await orderService
            .GetOrdersByStatusAsync(OrderStatus.Processing);

            Console.WriteLine($"Processing Orders Count: {processingOrders.Count}");

            foreach (var order in processingOrders)
            {
                Console.WriteLine($"Order Id: {order.Id} - Total: {order.TotalAmount}");
            }

            await orderService.UpdateOrderStatusAsync(1, OrderStatus.Processing);



        }


    }
}
