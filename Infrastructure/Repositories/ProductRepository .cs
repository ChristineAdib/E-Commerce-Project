using Application.Interfaces.Repository.Product_Repo;
using Domain.Entities;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class ProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<Product> Get_all_products()
        {
            return _context.Products;
        }

        public void Add_Product(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void Update_Product(Product product)
        {
            _context.Products.Update(product);
            _context.SaveChanges();
        }

        public void Delete_Product(Product product)
        {
            _context.Products.Remove(product);
            _context.SaveChanges();
        }
    }
}
