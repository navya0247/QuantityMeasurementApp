using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using QmaService.Data;
using QmaService.Entities;
using QmaService.Interfaces;

namespace QmaService.Repositories
{
    /// <summary>
    /// EF Core fallback repository — used when Redis is not available.
    /// </summary>
    public class EFQuantityRepository : IQuantityMeasurementRepository
    {
        private readonly QmaDbContext _db;

        public EFQuantityRepository(QmaDbContext db) => _db = db;

        public void Save(QuantityMeasurementEntity entity)
        {
            _db.QuantityMeasurements.Add(entity);
            _db.SaveChanges();
        }

        public List<QuantityMeasurementEntity> GetAll()
            => _db.QuantityMeasurements
                  .OrderByDescending(q => q.CreatedAt)
                  .ToList();

        public List<QuantityMeasurementEntity> GetByOperation(string operation)
            => _db.QuantityMeasurements
                  .Where(q => q.Operation == operation.ToUpper())
                  .OrderByDescending(q => q.CreatedAt)
                  .ToList();

        public List<QuantityMeasurementEntity> GetByMeasureType(string measureType)
            => _db.QuantityMeasurements
                  .Where(q => q.MeasureType == measureType.ToUpper())
                  .OrderByDescending(q => q.CreatedAt)
                  .ToList();

        public List<QuantityMeasurementEntity> GetErrored()
            => _db.QuantityMeasurements
                  .Where(q => q.IsError)
                  .OrderByDescending(q => q.CreatedAt)
                  .ToList();

        public int  GetTotalCount() => _db.QuantityMeasurements.Count();
        public void DeleteAll()
        {
            _db.QuantityMeasurements.RemoveRange(_db.QuantityMeasurements);
            _db.SaveChanges();
        }
    }
}
