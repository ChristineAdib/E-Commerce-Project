using Application.DTOs.ProductDTOs;
using Application.Interfaces.Repository.Product_Repo;
using Application.Interfaces.Services.Product_services;
using Domain.Entities;

namespace Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repo;

        public ProductService(IProductRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<GetProductDto>> GetAllProductsAsync()
        {
            var products = await _repo.GetAllAsync();

            if (!products.Any())
                throw new Exception("No products found");

            return products.Select(p => new GetProductDto
            {
                Id = p.Id,
                ProductName = p.ProductName,
                Price = p.Price,
                ImageUrl = p.ImageUrl,
                CategoryName = p.Category?.CategoryName,
                Description = p.Description,
                StockQuantity = p.StockQuantity
            });
        }
        public async Task<GetProductDto> GetProductByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid product id");

            var product = await _repo.GetByIdAsync(id);

            if (product == null)
                throw new Exception("Product not found");

            return new GetProductDto
            {
                Id = product.Id,
                ProductName = product.ProductName,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                CategoryName = product.Category?.CategoryName,
                Description = product.Description,
                StockQuantity = product.StockQuantity
            };
        }

        public async Task CreateProductAsync(CreateProductDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            if (string.IsNullOrWhiteSpace(dto.ProductName))
                throw new ArgumentException("Product name is required");

            if (dto.Price <= 0)
                throw new ArgumentException("Price must be greater than zero");

            if (dto.CategoryID <= 0)
                throw new ArgumentException("Invalid category");

            var product = new Product
            {
                ProductName = dto.ProductName.Trim(),
                Price = dto.Price,
                ImageUrl = dto.ImageUrl,
                CategoryID = dto.CategoryID,
                Description = dto.Description,
                StockQuantity = dto.StockQuantity
            };

            await _repo.AddAsync(product);
        }

        public async Task UpdateProductAsync(UpdateProductDto dto)
        {
            var existing = await _repo.GetByIdAsync(dto.Id);

            if (existing == null)
                throw new Exception("Product not found");

            if (dto.Price <= 0)
                throw new Exception("Invalid price");

            existing.ProductName = dto.ProductName;
            existing.Price = dto.Price;
            existing.ImageUrl = dto.ImageUrl;
            existing.CategoryID = dto.CategoryID;
            existing.Description = dto.Description;
            existing.StockQuantity = dto.StockQuantity;

            await _repo.UpdateAsync(existing);
        }

        public async Task DeleteProductAsync(int productId)
        {
            var existing = await _repo.GetByIdAsync(productId);

            if (existing == null)
                throw new Exception("Product not found");

            await _repo.DeleteAsync(existing);
        }
    }
}