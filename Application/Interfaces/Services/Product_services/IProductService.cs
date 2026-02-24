using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Services.Product_services
{
    public interface IProductService
    {
        IEnumerable<Product> GetAllProducts();

        void CreateProduct(Product product);

        void UpdateProduct(Product product);

        void DeleteProduct(Product product);
    }
}
