using System.Collections.Generic;
using QuantityMeasurementApp.ModelLayer.Entities;

namespace QuantityMeasurementApp.RepoLayer.Interfaces
{
    /// <summary>Data access contract for quantity measurement persistence.</summary>
    public interface IQuantityMeasurementRepository
    {
        void Save(QuantityMeasurementEntity entity);
        List<QuantityMeasurementEntity> GetAll();
        List<QuantityMeasurementEntity> GetByOperation(string operation);
        List<QuantityMeasurementEntity> GetByMeasureType(string measureType);
        List<QuantityMeasurementEntity> GetErrored();
        int  GetTotalCount();
        void DeleteAll();
    }
}
