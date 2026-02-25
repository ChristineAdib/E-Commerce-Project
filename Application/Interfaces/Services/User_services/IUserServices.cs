using Application.DTOs.UserDTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Services.User_services
{
    public interface IUserServices
    {
        public Task<GetUserDto> GetUserByIdAsync(int id);
        public Task<List<GetUserDto>> GetAllUsersAsync();
        public Task Register(AddUserDto user);
        public void Update(UpdataUserDto user);
        public void Delete(int id);
        public Task<GetUserDto> GetUserByEmailAsync(string Email);
        public Task<GetUserDto> Login(string username, string password);
    }
}
