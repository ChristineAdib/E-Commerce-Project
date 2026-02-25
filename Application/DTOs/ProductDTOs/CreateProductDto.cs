using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.ProductDTOs
{
    public class CreateProductDto
    {
        public string ProductName { get; set; }

        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

        public int CategoryID { get; set; }
        public string Description { get; set; }
        public int StockQuantity { get; set; }
    }
}
