using DeliverySite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeliverySite.ModelView
{
    public class ProductViewModel
    {
        public Product product { get; set; }
        public List<Product> products { get; set; }
        public User user { get; set; }

    }
}