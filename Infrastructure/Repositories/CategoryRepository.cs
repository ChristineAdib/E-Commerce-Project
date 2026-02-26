using Application.Interfaces.Repository.Ctegory_Repo;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class CategoryRepository: ICategoryReposirory
    {
        public ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }



        public IQueryable<Category> Get_all_category()
        {
            return _context.Categories.Include(c => c.Products);
        }
        public void add_Category(Category catgory)
        {

            _context.Add(catgory);
            _context.SaveChanges();

        }

        public void Update_Category(Category catgory)
        {

            _context.Update(catgory);
            _context.SaveChanges();

        }
        public void Delete_Category(Category catgory)
        {

            _context.Remove(catgory);
            _context.SaveChanges();

        }

    }
}
