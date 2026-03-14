using QuantityMeasurementApp.ModelLayer.DTO;

namespace QuantityMeasurementApp.BusinessLayer.Interfaces
{
    /// <summary>
    /// Defines business operations for quantity measurement.
    /// </summary>
    public interface IQuantityMeasurementService
    {
        bool Compare(QuantityDTO firstQuantity, QuantityDTO secondQuantity);

        QuantityDTO Convert(QuantityDTO sourceQuantity, object targetUnit);

        QuantityDTO Add(QuantityDTO firstQuantity, QuantityDTO secondQuantity, object targetUnit);

        QuantityDTO Subtract(QuantityDTO firstQuantity, QuantityDTO secondQuantity);

        double Divide(QuantityDTO firstQuantity, QuantityDTO secondQuantity);
    }
}