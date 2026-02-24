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

        public void Delete(int id)
        {
            var user = _userRepository.GetUserById(id);
            if (user == null)
                Console.WriteLine("This user not found.");
            _userRepository.Delete(id);
            Console.WriteLine($"{user.UserName} Deleted ");
        }

        //for admin
        public List<GetUserDto> GetAllUsers()
        {
            List<User> users = _userRepository.GetAllUsers();
            List<GetUserDto> userDtos = new();
            foreach(var u in users)
            {
                var user=new GetUserDto()
                {
                    UserName = u.UserName,
                    Email = u.Email
                };
                userDtos.Add(user);
            }
            return userDtos;
        }

        public GetUserDto GetUserByEmail(string Email)
        {
            var user = _userRepository.GetUserByEmail(Email);
            if (user == null)
                return null;

            var userDto = new GetUserDto() { UserName = user.UserName, Email = user.Email };
            return userDto;
        }

        public GetUserDto GetUserById(int id)
        {
            var user = _userRepository.GetUserById(id);
            if (user == null)
                return null;

            var userDto = new GetUserDto() { UserName = user.UserName, Email = user.Email };
            return userDto;
        }

        public GetUserDto Login(string username, string password)
        {
            var user = _userRepository.GetUserByUsername(username);
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

            var userDto = new GetUserDto() { UserName = user.UserName, Email = user.Email };
            return userDto;
        }

        public void Register(AddUserDto user)
        {
            var u = new User
            {
                UserName = user.UserName,
                Email = user.Email,
                Password = user.Password,
                IsAdmin = false
            };

             _userRepository.Add(u);

            var cart = new Cart { UserId = u.Id };
             _cartRepo.AddAsync(cart);
        }

        public void Update(UpdataUserDto userdto)
        {
            var existing = _userRepository.GetUserById(userdto.Id);
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
