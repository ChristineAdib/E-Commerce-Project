using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Order
    {
        public int Id { set; get; }
        public int UserId { set; get; }
        public DateTime OrderDate { set; get; }
        public OrderStatus Status { set; get; }
        public decimal TotalAmount { set; get; }
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
