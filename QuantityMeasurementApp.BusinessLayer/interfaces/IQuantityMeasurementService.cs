using System.Collections.Generic;
using QuantityMeasurementApp.ModelLayer.DTO;

namespace QuantityMeasurementApp.BusinessLayer.Interfaces
{
    /// <summary>Contract for all quantity measurement operations.</summary>
    public interface IQuantityMeasurementService
    {
        QuantityMeasurementDTO Compare(QuantityDTO first, QuantityDTO second);
        QuantityMeasurementDTO Convert(QuantityDTO source, object targetUnit);
        QuantityMeasurementDTO Add(QuantityDTO first, QuantityDTO second, object targetUnit);
        QuantityMeasurementDTO Subtract(QuantityDTO first, QuantityDTO second);
        QuantityMeasurementDTO Divide(QuantityDTO first, QuantityDTO second);

        List<QuantityMeasurementDTO> GetAllMeasurements();
        List<QuantityMeasurementDTO> GetByOperation(string operation);
        List<QuantityMeasurementDTO> GetByMeasureType(string measureType);
        List<QuantityMeasurementDTO> GetErrored();
        int  GetTotalCount();
        void DeleteAll();
    }
}
