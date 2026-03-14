using System.Collections.Generic;
using QuantityMeasurementApp.ModelLayer.Entities;
using QuantityMeasurementApp.RepoLayer.Interfaces;

namespace QuantityMeasurementApp.RepoLayer.Repositories
{
    /// <summary>
    /// Singleton repository storing measurement
    /// operations in memory.
    /// </summary>
    public class QuantityMeasurementCacheRepository : IQuantityMeasurementRepository
    {
        private static QuantityMeasurementCacheRepository instance;

        private List<QuantityMeasurementEntity> storage =
            new List<QuantityMeasurementEntity>();

        private QuantityMeasurementCacheRepository() { }

        public static QuantityMeasurementCacheRepository GetInstance()
        {
            if (instance == null)
                instance = new QuantityMeasurementCacheRepository();

            return instance;
        }

        public void Save(QuantityMeasurementEntity entity)
        {
            storage.Add(entity);
        }

        public List<QuantityMeasurementEntity> GetAll()
        {
            return storage;
        }
    }
}