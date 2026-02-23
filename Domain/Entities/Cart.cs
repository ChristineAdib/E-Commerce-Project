using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Cart:BaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public List<CartItem> CartItems { get; set; }
    }
}
