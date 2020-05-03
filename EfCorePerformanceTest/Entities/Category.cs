using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfCorePerformanceTest.Entities
{
    public class Category
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public DateTime CreateDate { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
