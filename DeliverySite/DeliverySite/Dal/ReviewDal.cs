using DeliverySite.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DeliverySite.Dal
{
    public class ReviewDal: DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Review>().ToTable("Reviews");
        }
        public DbSet<Review> Review { get; set; }

    }
}