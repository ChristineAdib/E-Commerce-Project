using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.OrderDTOs
{
    public class CreateOrderDto
    {
        public int UserId { set; get; }
        public List<CreateOrderItemDto> Items { set; get; } = new();
    }
}
