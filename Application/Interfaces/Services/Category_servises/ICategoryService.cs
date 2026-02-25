using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Services.Category_servises
{
    public interface ICategoryService
    {
        List<Domain.Entities.Category> GetAll();
        void AddCategory(Application.DTOs.CategoryDTOs.CreateCategoryDto category);
        void UpdateCategory(Application.DTOs.CategoryDTOs.UpdateCategoryDto category);
        void DeleteCategory(int id);
    }
}
