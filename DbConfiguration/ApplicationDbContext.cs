using Microsoft.EntityFrameworkCore;
using Star_Properties.Model.EntityModel;
using System;

namespace Star_Properties.DbConfiguration
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<UserMaster> UserMaster { get; set; }
        public DbSet<PropertiesDetailsMaster> PropertiesDetailsMaster { get; set; }
        public DbSet<CustomerContactMaster> CustomerContactMaster { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PropertiesDetailsMaster>()
                .Property(p => p.ImageUrls)
                .HasColumnType("text");

            base.OnModelCreating(modelBuilder);
        }
    }
}
