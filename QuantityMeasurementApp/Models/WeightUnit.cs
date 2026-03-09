using System;

namespace QuantityMeasurementApp.Models
{
    /// <summary>
    /// UC10: Weight units implementing IMeasurable
    /// </summary>
    public enum WeightUnit
    {
        KILOGRAM,
        GRAM,
        POUND
    }

    public static class WeightUnitExtensions
    {
        /// <summary>
        /// Returns conversion factor relative to KG
        /// </summary>
        public static double GetConversionFactor(this WeightUnit unit)
        {
            return unit switch
            {
                WeightUnit.KILOGRAM => 1,
                WeightUnit.GRAM => 0.001,
                WeightUnit.POUND => 0.453592,
                _ => throw new ArgumentException("Invalid weight unit")
            };
        }

        /// <summary>
        /// Convert value to base unit (KG)
        /// </summary>
        public static double ConvertToBaseUnit(this WeightUnit unit, double value)
        {
            return value * unit.GetConversionFactor();
        }

        /// <summary>
        /// Convert from base unit
        /// </summary>
        public static double ConvertFromBaseUnit(this WeightUnit unit, double baseValue)
        {
            return baseValue / unit.GetConversionFactor();
        }

        /// <summary>
        /// Returns unit name
        /// </summary>
        public static string GetUnitName(this WeightUnit unit)
        {
            return unit.ToString();
        }
    }
}