using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.CategoryDTOs
{
    public class UpdateCategoryDto
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
    }
}
