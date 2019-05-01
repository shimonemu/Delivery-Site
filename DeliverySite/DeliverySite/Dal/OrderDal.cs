using DeliverySite.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DeliverySite.Dal
{
    public class OrderDal: DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
           base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Order>().ToTable("Orders");
        }
        public DbSet<Order> Order { get; set; }
    }
}