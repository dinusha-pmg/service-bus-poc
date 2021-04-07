using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceBus.Poc.Data
{
   public class DataContext:DbContext
    {
        public DataContext()
        {

        }
        public DataContext(DbContextOptions options):base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if(!options.IsConfigured)
            {
                options.UseSqlServer("Server=TI-NB-366;Database=esb_poc;Trusted_Connection=True;MultipleActiveResultSets=true");

            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<AuditTrail> AuditTrails { get; set; }
    }
}
