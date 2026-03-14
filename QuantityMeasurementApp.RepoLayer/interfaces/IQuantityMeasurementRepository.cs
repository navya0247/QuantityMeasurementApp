using QuantityMeasurementApp.ModelLayer.Entities;
using System.Collections.Generic;

namespace QuantityMeasurementApp.RepoLayer.Interfaces
{
    public interface IQuantityMeasurementRepository
    {
        void Save(QuantityMeasurementEntity entity);
        List<QuantityMeasurementEntity> GetAll();
        List<QuantityMeasurementEntity> GetByOperation(string operation);
        List<QuantityMeasurementEntity> GetByMeasureType(string measureType);
        List<QuantityMeasurementEntity> GetFullHistory();
        int GetTotalCount();
        void DeleteAll();
        string GetPoolStats();
    }
}