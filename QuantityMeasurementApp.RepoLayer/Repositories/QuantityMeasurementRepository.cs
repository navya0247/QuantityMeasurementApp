using System.Collections.Generic;
using System.Linq;
using QuantityMeasurementApp.ModelLayer.Entities;
using QuantityMeasurementApp.RepoLayer.Data;
using QuantityMeasurementApp.RepoLayer.Interfaces;

namespace QuantityMeasurementApp.RepoLayer.Repositories
{
    /// <summary>EF Core implementation using LINQ to Entities — no raw SQL.</summary>
    public class QuantityMeasurementRepository : IQuantityMeasurementRepository
    {
        private readonly AppDbContext _context;

        public QuantityMeasurementRepository(AppDbContext context) => _context = context;

        /// <summary>Saves a new measurement record to the database.</summary>
        public void Save(QuantityMeasurementEntity entity)
        {
            _context.QuantityMeasurements.Add(entity);
            _context.SaveChanges();
        }

        /// <summary>Returns all records ordered by most recent first.</summary>
        public List<QuantityMeasurementEntity> GetAll()
            => _context.QuantityMeasurements
                       .OrderByDescending(q => q.CreatedAt)
                       .ToList();

        /// <summary>Filters records by operation type using LINQ.</summary>
        public List<QuantityMeasurementEntity> GetByOperation(string operation)
            => _context.QuantityMeasurements
                       .Where(q => q.Operation == operation.ToUpper())
                       .OrderByDescending(q => q.CreatedAt)
                       .ToList();

        /// <summary>Filters records by measurement type using LINQ.</summary>
        public List<QuantityMeasurementEntity> GetByMeasureType(string measureType)
            => _context.QuantityMeasurements
                       .Where(q => q.MeasureType == measureType.ToUpper())
                       .OrderByDescending(q => q.CreatedAt)
                       .ToList();

        /// <summary>Returns all records where an error occurred.</summary>
        public List<QuantityMeasurementEntity> GetErrored()
            => _context.QuantityMeasurements
                       .Where(q => q.IsError)
                       .OrderByDescending(q => q.CreatedAt)
                       .ToList();

        /// <summary>Returns total count of all records.</summary>
        public int GetTotalCount()
            => _context.QuantityMeasurements.Count();

        /// <summary>Deletes all measurement records.</summary>
        public void DeleteAll()
        {
            _context.QuantityMeasurements.RemoveRange(_context.QuantityMeasurements);
            _context.SaveChanges();
        }
    }
}
