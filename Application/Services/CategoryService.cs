using Application.DTOs.CategoryDTOs;
using Application.Interfaces.Repository.Ctegory_Repo;
using Application.Interfaces.Services.Category_servises;
using Domain.Entities;
using Mapster;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Services
{
    public class CategoryService : ICategoryService
    {
        public ICategoryReposirory _categoryRepository;
        public CategoryService(ICategoryReposirory categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public List<Category> GetAll()
        {
            return _categoryRepository.Get_all_category().ToList();
        }

        public void AddCategory(CreateCategoryDto category)
        {
            if (string.IsNullOrWhiteSpace(category.CategoryName))
            {
                throw new Exception("Category name cannot be empty.........");
            }

            var isExist = _categoryRepository.Get_all_category()
                .Any(x => x.CategoryName == category.CategoryName);

            if (isExist)
            {
                throw new Exception("Category name already exists.");
            }

            var newcategory = category.Adapt<Category>();

            _categoryRepository.add_Category(newcategory);
        }

        public void UpdateCategory(UpdateCategoryDto category)
        {

            if (category == null || category.Id <= 0)
            {
                throw new Exception("Invalid category data!!");
            }


            if (string.IsNullOrWhiteSpace(category.CategoryName))
            {
                throw new Exception("Category name cannot be empty!!");
            }

            bool isnamefound = _categoryRepository.Get_all_category()
                .Any(c => c.CategoryName == category.CategoryName
                      && c.Id != category.Id);

            if (isnamefound)
            {
                throw new Exception("Another category is already using this name.");
            }

            var categoryEntity = category.Adapt<Category>();
            _categoryRepository.add_Category(categoryEntity);
        }

        public void DeleteCategory(int id)
        {

            var category = _categoryRepository.Get_all_category()
                .FirstOrDefault(c => c.Id == id);

            if (category == null)
            {
                throw new Exception("Category not found.");
            }



            _categoryRepository.Delete_Category(category);
        }


    }
}