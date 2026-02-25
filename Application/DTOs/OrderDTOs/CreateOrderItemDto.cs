using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.OrderDTOs
{
    public class CreateOrderItemDto
    {
        public int ProductId { set; get; }
        public int Quantity { set; get; }
    }
}
