using QuantityMeasurementApp.ModelLayer.Entities;
using System.Collections.Generic;

namespace QuantityMeasurementApp.RepoLayer.Interfaces
{
    /// <summary>
    /// Repository interface defining data storage operations.
    /// </summary>
    public interface IQuantityMeasurementRepository
    {
        void Save(QuantityMeasurementEntity entity);

        List<QuantityMeasurementEntity> GetAll();
    }
}