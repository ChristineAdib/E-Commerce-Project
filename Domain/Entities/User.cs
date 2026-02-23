using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class User:BaseEntity
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        public Cart Cart { get; set; }
        public int CartId { get; set; }
        public List<Order> Orders { get; set; }
    }
}
