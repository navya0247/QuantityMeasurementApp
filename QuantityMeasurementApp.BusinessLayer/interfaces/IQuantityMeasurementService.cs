using QuantityMeasurementApp.ModelLayer.DTO;
using QuantityMeasurementApp.ModelLayer.Entities;
using System.Collections.Generic;

namespace QuantityMeasurementApp.BusinessLayer.Interfaces
{
    public interface IQuantityMeasurementService
    {
        bool Compare(QuantityDTO firstQuantity, QuantityDTO secondQuantity);
        QuantityDTO Convert(QuantityDTO sourceQuantity, object targetUnit);
        QuantityDTO Add(QuantityDTO firstQuantity, QuantityDTO secondQuantity, object targetUnit);
        QuantityDTO Subtract(QuantityDTO firstQuantity, QuantityDTO secondQuantity);
        double Divide(QuantityDTO firstQuantity, QuantityDTO secondQuantity);
        List<QuantityMeasurementEntity> GetAllMeasurements();
        List<QuantityMeasurementEntity> GetByOperation(string operation);
        List<QuantityMeasurementEntity> GetByMeasureType(string measureType);
        int GetTotalCount();
        void DeleteAll();
        string GetPoolStats();
    }
}