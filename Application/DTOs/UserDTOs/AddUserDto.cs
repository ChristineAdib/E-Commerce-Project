using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.UserDTOs
{
    public class AddUserDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
    }
}
