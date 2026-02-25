using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.ProductDTOs
{
    public class GetProductDto
    {
        public int Id { get; set; }

        public string ProductName { get; set; }

        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

        public string CategoryName { get; set; }
        public string Description { get; set; }
        public int StockQuantity { get; set; }
    }
}
