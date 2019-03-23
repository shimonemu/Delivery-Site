using DeliverySite.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DeliverySite.Dal
{
    public class CompanyDal:DbContext
    {

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Company>().ToTable("Companies");
        }
        public DbSet<Company> Company { get; set; }
    }
}