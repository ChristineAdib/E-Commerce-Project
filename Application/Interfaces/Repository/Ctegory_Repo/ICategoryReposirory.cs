using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Repository.Ctegory_Repo
{
    public interface ICategoryReposirory
    {
        public IQueryable<Category> Get_all_category();
       
        public void add_Category(Category catgory);


        public void Update_Category(Category catgory);

        public void Delete_Category(Category catgory);
        
    }
}
