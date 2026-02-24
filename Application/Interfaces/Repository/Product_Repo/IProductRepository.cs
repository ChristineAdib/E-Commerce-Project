using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Repository.Product_Repo
{
    public interface IProductRepository
    {
        IQueryable<Product> Get_all_products();

        void Add_Product(Product product);

        void Update_Product(Product product);

        void Delete_Product(Product product);
    }

}
