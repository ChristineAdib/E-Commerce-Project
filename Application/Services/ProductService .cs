using Application.Interfaces.Repository.Product_Repo;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Services
{
    public class ProductService
    {
        private readonly IProductRepository _repo;

        public ProductService(IProductRepository repo)
        {
            _repo = repo;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _repo.Get_all_products().ToList();
        }

        public void CreateProduct(Product product)
        {
            _repo.Add_Product(product);
        }

        public void UpdateProduct(Product product)
        {
            _repo.Update_Product(product);
        }

        public void DeleteProduct(Product product)
        {
            _repo.Delete_Product(product);
        }
    }
}
