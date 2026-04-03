using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Star_Properties.Model.EntityModel;

namespace Star_Properties.DbConfiguration
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<UserMaster> UserMaster { get; set; }
        public DbSet<PropertiesDetailsMaster> PropertiesDetailsMaster { get; set; }
        public DbSet<CustomerContactMaster> CustomerContactMaster { get; set; }
        public DbSet<CustomerContactAudit> CustomerContactAudit { get; set; }
        public DbSet<EmailLog> EmailLog { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PropertiesDetailsMaster>()
                .Property(p => p.ImageUrls)
                .HasColumnType("text");

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            NormalizeDateTimesToUtc();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            NormalizeDateTimesToUtc();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            NormalizeDateTimesToUtc();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            NormalizeDateTimesToUtc();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void NormalizeDateTimesToUtc()
        {
            foreach (var entry in ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
            {
                foreach (var property in entry.Properties)
                {
                    if (property.Metadata.ClrType == typeof(DateTime))
                    {
                        NormalizeDateTimeProperty(property);
                    }
                    else if (property.Metadata.ClrType == typeof(DateTime?))
                    {
                        NormalizeNullableDateTimeProperty(property);
                    }
                }
            }
        }

        private static void NormalizeDateTimeProperty(PropertyEntry property)
        {
            if (property.CurrentValue is not DateTime dateTime)
                return;

            property.CurrentValue = dateTime.Kind switch
            {
                DateTimeKind.Utc => dateTime,
                DateTimeKind.Local => dateTime.ToUniversalTime(),
                _ => DateTime.SpecifyKind(dateTime, DateTimeKind.Utc)
            };
        }

        private static void NormalizeNullableDateTimeProperty(PropertyEntry property)
        {
            if (property.CurrentValue is not DateTime dateTime)
                return;

            property.CurrentValue = dateTime.Kind switch
            {
                DateTimeKind.Utc => dateTime,
                DateTimeKind.Local => dateTime.ToUniversalTime(),
                _ => DateTime.SpecifyKind(dateTime, DateTimeKind.Utc)
            };
        }
    }
}
