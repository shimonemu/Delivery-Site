using DeliverySite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeliverySite.ModelView
{
    public class DoctorViewModel
    {
        public Manager manager { get; set; }
        public List<Manager> managers { get; set; }
    }
}