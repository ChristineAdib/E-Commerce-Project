using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.ProductDTOs
{
    public class GetProductDto
    {
        public int ProductID { get; set; }

        public string ProductName { get; set; }

        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

        public string CategoryName { get; set; }
    }
}
