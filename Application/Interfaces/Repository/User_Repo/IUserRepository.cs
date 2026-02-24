using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Repository.User_Repo
{
    public interface IUserRepository
    {
        public User GetUserById(int id);
        public List<User> GetAllUsers();
        public void Add(User user);
        public void Update(User user);
        public void Delete(int id);
        public User GetUserByEmail(string Email);
        public User GetUserByUsername(string username);
        public User Login(string username, string password);
    }
}
