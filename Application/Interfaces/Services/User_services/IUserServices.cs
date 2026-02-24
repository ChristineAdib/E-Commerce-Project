using Application.DTOs.UserDTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Services.User_services
{
    public interface IUserServices
    {
        public GetUserDto GetUserById(int id);
        public List<GetUserDto> GetAllUsers();
        public void Register(AddUserDto user);
        public void Update(UpdataUserDto user);
        public void Delete(int id);
        public GetUserDto GetUserByEmail(string Email);
        public GetUserDto Login(string username, string password);
    }
}
