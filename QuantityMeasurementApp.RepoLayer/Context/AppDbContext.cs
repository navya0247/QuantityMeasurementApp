using Microsoft.EntityFrameworkCore;
using QuantityMeasurementApp.ModelLayer.Entities;

namespace QuantityMeasurementApp.RepoLayer.Data
{
    /// <summary>EF Core DbContext managing Users and QuantityMeasurements tables.</summary>
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<UserEntity>                Users                { get; set; }
        public DbSet<QuantityMeasurementEntity> QuantityMeasurements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Unique email index on users
            modelBuilder.Entity<UserEntity>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Indexes for fast filtering on measurements
            modelBuilder.Entity<QuantityMeasurementEntity>()
                .HasIndex(q => q.Operation)
                .HasDatabaseName("IX_Measurements_Operation");

            modelBuilder.Entity<QuantityMeasurementEntity>()
                .HasIndex(q => q.MeasureType)
                .HasDatabaseName("IX_Measurements_MeasureType");

            modelBuilder.Entity<QuantityMeasurementEntity>()
                .HasIndex(q => q.IsError)
                .HasDatabaseName("IX_Measurements_IsError");
        }
    }
}
