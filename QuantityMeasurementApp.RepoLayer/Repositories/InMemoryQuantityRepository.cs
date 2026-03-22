using System.Collections.Generic;
using System.Linq;
using QuantityMeasurementApp.ModelLayer.Entities;
using QuantityMeasurementApp.RepoLayer.Interfaces;

namespace QuantityMeasurementApp.RepoLayer.Repositories
{
    /// <summary>
    /// Simple in-memory repository for the console app.
    /// </summary>
    public class InMemoryQuantityRepository : IQuantityMeasurementRepository
    {
        private readonly List<QuantityMeasurementEntity> _store = new();

        public void Save(QuantityMeasurementEntity entity) => _store.Add(entity);

        public List<QuantityMeasurementEntity> GetAll()
            => _store.OrderByDescending(q => q.CreatedAt).ToList();

        public List<QuantityMeasurementEntity> GetByOperation(string operation)
            => _store.Where(q => q.Operation == operation.ToUpper())
                     .OrderByDescending(q => q.CreatedAt).ToList();

        public List<QuantityMeasurementEntity> GetByMeasureType(string measureType)
            => _store.Where(q => q.MeasureType == measureType.ToUpper())
                     .OrderByDescending(q => q.CreatedAt).ToList();

        public List<QuantityMeasurementEntity> GetErrored()
            => _store.Where(q => q.IsError)
                     .OrderByDescending(q => q.CreatedAt).ToList();

        public int  GetTotalCount() => _store.Count;
        public void DeleteAll()     => _store.Clear();
    }
}
