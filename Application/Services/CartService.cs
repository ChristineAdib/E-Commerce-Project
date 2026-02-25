using Application.DTOs.CartDTOs;
using Application.Interfaces.Repository.Cart_Repo;
using Application.Interfaces.Repository.Product_Repo;
using Application.Interfaces.Repository.User_Repo;
using Application.Interfaces.Services.Cart_services;
using Domain.Entities;
using Mapster;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Services
{
    public class CartService : ICartServices
    {
        private readonly ICartRepository _cartRepo;
        private readonly ICartItemRepository _cartItemRepo;
        private readonly IUserRepository _userRepo;
        private readonly IProductRepository _productRepo;

        public CartService(ICartRepository cartRepo, ICartItemRepository cartItemRepo,IUserRepository userRepository, IProductRepository productRepo)
        {
            _cartRepo = cartRepo;
            _cartItemRepo = cartItemRepo;
            _userRepo = userRepository;
            _productRepo = productRepo;
        }
        public async Task AddToCartAsync(int userId, int productId, int quantity)
        {
            await ValidateUser(userId);

            if (quantity <= 0)
                throw new Exception("Quantity must be greater than zero");

            var product = await _productRepo.GetByIdAsync(productId);
            if (product == null)
                throw new Exception("Product not found");

            if (product.StockQuantity <= 0)
                throw new Exception("Product out of stock");

            var cart = await _cartRepo.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                await _cartRepo.AddAsync(cart);
            }

            var existingItem = await _cartItemRepo.GetByCartAndProductAsync(cart.Id, productId);
            int total = quantity;

            if (existingItem is not null)
                total += existingItem.Quantity;

            if(total>product.StockQuantity)
                throw new Exception("Requested quantity exceeds available stock");

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
                _cartItemRepo.Update(existingItem);
            }

            else
            {
                var newItem = new CartItem
                {
                    CartId = cart.Id,
                    ProductId = productId,
                    Quantity = quantity
                };
                await _cartItemRepo.AddAsync(newItem);
            }
        }

        public async Task ClearCartAsync(int userId)
        {
            var cart = await _cartRepo.GetCartByUserIdAsync(userId);
            foreach(var i in cart.CartItems.ToList())
            {
                _cartItemRepo.Delete(i);
            }
        }

        public async Task<CartDto> GetCartAsync(int userId)
        {
            await ValidateUser(userId);
            var cart = await _cartRepo.GetCartByUserIdAsync(userId);

            if (cart is null)
                throw new Exception("Cart not found");

            return cart.Adapt<CartDto>();
        }

        public async Task<decimal> GetCartTotalAsync(int userId)
        {
            var cart = await _cartRepo.GetCartByUserIdAsync(userId);
            return cart.CartItems.Sum(i => i.Product.Price * i.Quantity);
        }

        public async Task RemoveFromCartAsync(int userId, int productId)
        {
            await ValidateUser(userId);
            var cart = await _cartRepo.GetCartByUserIdAsync(userId);
            var item = await _cartItemRepo.GetByCartAndProductAsync(cart.Id, productId);
            if (item == null)
                throw new Exception("Item not found in cart");

            _cartItemRepo.Delete(item);
        }

        public async Task UpdateQuantityAsync(int userId, int productId, int quantity)
        {
            await ValidateUser(userId);
            var cart = await _cartRepo.GetCartByUserIdAsync(userId);

            var item =
                await _cartItemRepo.GetByCartAndProductAsync(cart.Id, productId);

            if (item == null)
                throw new Exception("Item not found in cart");

            if (quantity <= 0)
            {
                _cartItemRepo.Delete(item);
                return;
            }

            var product = await _productRepo.GetByIdAsync(productId);

            if (product == null)
                throw new Exception("Product not found");

            if (quantity > product.StockQuantity)
                throw new Exception("Quantity exceeds stock");

            item.Quantity = quantity;
            _cartItemRepo.Update(item);
        }

        private async Task ValidateUser(int userId)
        {
            var user =await _userRepo.GetUserByIdAsync(userId);

            if (user == null)
                throw new Exception("User not found");

            if (user.IsAdmin != false)
                throw new Exception("Only customers can use cart");
        }
    }
}
