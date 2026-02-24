using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Product:BaseEntity
    {
        public string ProductName { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        // FK
        public int CategoryID { get; set; }

        // Navigation
        public Category Category { get; set; }
        public List<CartItem> cartItems { get; set; }
    }
}
