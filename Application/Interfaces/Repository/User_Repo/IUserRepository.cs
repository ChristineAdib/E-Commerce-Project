using Application.DTOs.UserDTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Repository.User_Repo
{
    public interface IUserRepository
    {
        public Task<User> GetUserByIdAsync(int id);
        public Task<List<User>> GetAllUsersAsync();
        public Task AddAsync(User user);
        public void Update(User user);
        public void Delete(int id);
        public Task<User> GetUserByEmailAsync(string Email);
        public Task<User> GetUserByUsernameAsync(string username);
        public Task<User> Login(string username, string password);
    }
}
