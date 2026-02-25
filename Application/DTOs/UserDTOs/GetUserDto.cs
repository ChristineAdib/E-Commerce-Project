using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.UserDTOs
{
    public class GetUserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
    }
}
