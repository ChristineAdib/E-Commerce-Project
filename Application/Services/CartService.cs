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
            if (quantity <= 0)
            {
                Console.WriteLine("Quantity must be greater than zero");
            }

            var cart = await _cartRepo.GetCartByUserIdAsync(userId);
            var existingItem = await _cartItemRepo.GetByCartAndProductAsync(cart.Id, productId);

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
            var cart = await _cartRepo.GetCartByUserIdAsync(userId);

            if (cart is null)
            {
                Console.WriteLine("Cart Not Found");
                return null;
            }

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

            var product = await _productRepo.get(productId);

            if (quantity > product.Stock)
                throw new Exception("Quantity exceeds stock");

            item.Quantity = quantity;
            _cartItemRepo.Update(item);
        }

        private async Task ValidateUser(int userId)
        {
            var user = _userRepo.GetUserById(userId);

            if (user == null)
                throw new Exception("User not found");

            if (user.IsAdmin != false)
                throw new Exception("Only customers can use cart");
        }
    }
}
