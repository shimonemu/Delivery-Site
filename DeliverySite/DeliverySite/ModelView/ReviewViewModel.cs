using DeliverySite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeliverySite.ModelView
{
    public class ReviewViewModel
    {
        public Order order { get; set; }
        public List<Order> orders { get; set; }
        public User user { get; set; }
    }
}