namespace QuantityMeasurementApp.Models
{
    /// <summary>
    /// UC10: Interface that defines behaviour for all measurement units
    /// </summary>
    public interface IMeasurable
    {
        /// <summary>
        /// Returns conversion factor relative to base unit
        /// </summary>
        double GetConversionFactor();

        /// <summary>
        /// Convert value to base unit
        /// </summary>
        double ConvertToBaseUnit(double value);

        /// <summary>
        /// Convert value from base unit
        /// </summary>
        double ConvertFromBaseUnit(double baseValue);

        /// <summary>
        /// Returns unit name
        /// </summary>
        string GetUnitName();
    }
}