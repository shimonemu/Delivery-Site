using DeliverySite.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DeliverySite.Dal
{
    public class ManagerDal:DbContext
    {

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Manager>().ToTable("Managers");
        }
        public DbSet<Manager> Manager { get; set; }

    }
}