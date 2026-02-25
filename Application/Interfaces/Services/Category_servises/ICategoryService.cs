using Application.DTOs.CategoryDTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Services.Category_servises
{
    public interface ICategoryService
    {
        List<GetCategoryDto> GetAll();
        void AddCategory(CreateCategoryDto categoryDto);
        void UpdateCategory(UpdateCategoryDto categoryDto);
        void DeleteCategory(int id);
    }
}
