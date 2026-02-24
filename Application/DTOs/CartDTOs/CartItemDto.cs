using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.CartDTOs
{
    public class CartItemDto
    {
        public string ProductName { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public decimal Total => Price * Quantity;
    }
}
