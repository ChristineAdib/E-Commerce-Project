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

            var orderService = new OrderService(orderRepo, productRepo);

           

            var category = new Category
            {
                CategoryName = "Test Category",
                CategoryDescription = "Demo Description"
            };

            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();


            var product = new Product
            {
                ProductName = "Test Product",
                Price = 100,
                Description = "Demo",
                ImageUrl = "img.jpg",
                CategoryID = category.Id,
                StockQuantity = 10
            };

            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();

           

            var orderDto = new CreateOrderDto
            {
                UserId = 1,
                Items =
                {
                    new CreateOrderItemDto
                    {
                        ProductId = product.Id,
                        Quantity = 2
                    }
                }
            };

            var createdOrder = await orderService.CreateOrderAsync(orderDto);

            Console.WriteLine($"Product Added with Id: {product.Id}");
            Console.WriteLine($"Order Created with Id: {createdOrder.Id}");
            Console.WriteLine($"Total Amount: {createdOrder.TotalAmount}");

            Console.ReadLine();
        }
    }
}
