using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.OrderDTOs
{
    public class OrderItemDto
    {
        public int ProductId{ set; get; }
        public string ProductName { set; get; } = string.Empty;
        public int Quantity{ set; get; }
        public decimal Price{ set; get; }
        public decimal SubTotal{ set; get; }
    }
}
