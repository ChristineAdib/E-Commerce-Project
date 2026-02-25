using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.OrderDTOs
{
    public class OrderDto
    {
        public int Id { set; get; }
        public int UserId { set; get; }
        public DateTime OrderDate { set; get; }

        public string Status { set; get; }
        public decimal TotalAmount { set; get; }
        public List<OrderItemDto> Items { set; get; } = new();

    }
}
