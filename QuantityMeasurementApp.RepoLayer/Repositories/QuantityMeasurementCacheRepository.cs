using QuantityMeasurementApp.ModelLayer.Entities;
using QuantityMeasurementApp.RepoLayer.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace QuantityMeasurementApp.RepoLayer.Repositories
{
    public class QuantityMeasurementCacheRepository : IQuantityMeasurementRepository
    {
        private static QuantityMeasurementCacheRepository _instance;
        private readonly List<QuantityMeasurementEntity> _cache = new();

        private QuantityMeasurementCacheRepository() { }

        public static QuantityMeasurementCacheRepository GetInstance()
        {
            _instance ??= new QuantityMeasurementCacheRepository();
            return _instance;
        }

        public void Save(QuantityMeasurementEntity entity) =>
            _cache.Add(entity);

        public List<QuantityMeasurementEntity> GetAll() =>
            _cache.ToList();

        public List<QuantityMeasurementEntity> GetByOperation(string operation) =>
            _cache.Where(e => e.Operation == operation.ToUpper()).ToList();

        public List<QuantityMeasurementEntity> GetByMeasureType(string measureType) =>
            _cache.Where(e => e.MeasureType == measureType.ToUpper()).ToList();

        public List<QuantityMeasurementEntity> GetFullHistory() =>
            _cache.ToList();

        public int GetTotalCount() =>
            _cache.Count;

        public void DeleteAll() =>
            _cache.Clear();

        public string GetPoolStats() =>
            $"Cache Repository | Total records: {_cache.Count}";
    }
}