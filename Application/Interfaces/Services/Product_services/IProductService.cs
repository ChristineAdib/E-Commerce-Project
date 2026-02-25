using Application.DTOs.ProductDTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Services.Product_services
{
    public interface IProductService
    {
        Task<IEnumerable<GetProductDto>> GetAllProductsAsync();
        Task<GetProductDto> GetProductByIdAsync(int id);
        Task CreateProductAsync(CreateProductDto dto);

        Task UpdateProductAsync(UpdateProductDto dto);

        Task DeleteProductAsync(int productId);
    }
}
