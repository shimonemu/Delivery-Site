using DeliverySite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeliverySite.ModelView
{
    public class UserViewModel
    {
        public User user { get; set; }
        public List<User> users { get; set; }
    }
}