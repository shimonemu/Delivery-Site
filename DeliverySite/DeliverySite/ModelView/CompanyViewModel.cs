using DeliverySite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeliverySite.ModelView
{
    public class CompanyViewModel
    {
        public Company company { get; set; }
        public List<Company> companies { get; set; }
    }
}