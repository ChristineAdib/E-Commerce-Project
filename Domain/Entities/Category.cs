using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Category
    {
        public int CategoryID { get; set; }
        public string CategoryName{ get; set; }
        public string CategoryDescription{ get; set; }
        //public virtual List<Product> Products { get; set; }
    }
}
