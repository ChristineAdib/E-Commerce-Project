using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Category:BaseEntity
    {
        public string CategoryName{ get; set; }
        public string CategoryDescription{ get; set; }
        public virtual List<Product> Products { get; set; }
    }
}
