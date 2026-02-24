using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.CartDTOs
{
    public class CartDto
    {
        public int UserId { get; set; }
        public List<CartItemDto> Items { get; set; }

        public decimal GrandTotal => Items.Sum(i => i.Total);
    }
}
