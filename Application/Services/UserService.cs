using Application.DTOs.UserDTOs;
using Application.Interfaces.Repository.Cart_Repo;
using Application.Interfaces.Repository.User_Repo;
using Application.Interfaces.Services.User_services;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserService : IUserServices
    {
        public IUserRepository _userRepository;
        public ICartRepository _cartRepo;
        public UserService(IUserRepository userRepository,ICartRepository cartRepository)
        {
            _userRepository = userRepository;
            _cartRepo = cartRepository;
        }

        public async void Delete(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
                Console.WriteLine("This user not found.");
            _userRepository.Delete(id);
            Console.WriteLine($"{user.UserName} Deleted ");
        }

        //for admin
        public async Task<List<GetUserDto>> GetAllUsersAsync()
        {
            List<User> users =await _userRepository.GetAllUsersAsync();
            List<GetUserDto> userDtos = new();
            foreach(var u in users)
            {
                var user=new GetUserDto()
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    IsAdmin = u.IsAdmin
                };
                userDtos.Add(user);
            }
            return userDtos;
        }

        public async Task<GetUserDto> GetUserByEmailAsync(string Email)
        {
            var user =await _userRepository.GetUserByEmailAsync(Email);
            if (user == null)
                return null;

            var userDto = new GetUserDto() { Id = user.Id, UserName = user.UserName, Email = user.Email, IsAdmin = user.IsAdmin };
            return userDto;
        }

        public async Task<GetUserDto> GetUserByIdAsync(int id)
        {
            var user =await _userRepository.GetUserByIdAsync(id);
            if (user == null)
                return null;

            var userDto = new GetUserDto() { Id = user.Id, UserName = user.UserName, Email = user.Email, IsAdmin = user.IsAdmin };
            return userDto;
        }

        public async Task<GetUserDto> Login(string username, string password)
        {
            var user =await _userRepository.GetUserByUsernameAsync(username);
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                Console.WriteLine($"username and password is required");
                return null;
            }

            if(user.Password!=password||user is null)
            {
                Console.WriteLine("Invalid password or username.");
                return null;
            }

            _userRepository.Login(username, password);

            var userDto = new GetUserDto() { Id = user.Id, UserName = user.UserName, Email = user.Email, IsAdmin = user.IsAdmin };
            return userDto;
        }

        public async Task Register(AddUserDto user)
        {
            var u = new User
            {
                UserName = user.UserName,
                Email = user.Email,
                Password = user.Password,
                IsAdmin = false
            };

             await _userRepository.AddAsync(u);

            var cart = new Cart { UserId = u.Id };
             await _cartRepo.AddAsync(cart);
        }

        public async void Update(UpdataUserDto userdto)
        {
            var existing =await _userRepository.GetUserByIdAsync(userdto.Id);
            if (existing == null)
                Console.WriteLine("This user not found.");

            existing.UserName = userdto.UserName;
            existing.Password = userdto.Password;
            existing.Email = userdto.Email;
            _userRepository.Update(existing);
            Console.WriteLine("User updated.");
        }
    }
}
