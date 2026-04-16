using Microsoft.EntityFrameworkCore;
using QmaService.Entities;

namespace QmaService.Data
{
    public class QmaDbContext : DbContext
    {
        public QmaDbContext(DbContextOptions<QmaDbContext> options) : base(options) { }

        public DbSet<QuantityMeasurementEntity> QuantityMeasurements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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
