using System;

namespace QuantityMeasurementApp.Models
{
    /// <summary>
    /// UC10: Length units used with generic Quantity class
    /// </summary>
    public enum LengthUnit
    {
        FEET,
        INCHES,
        YARDS,
        CENTIMETERS
    }

    public static class LengthUnitExtensions
    {
        /// <summary>
        /// Returns conversion factor relative to FEET
        /// </summary>
        public static double GetConversionFactor(this LengthUnit unit)
        {
            return unit switch
            {
                LengthUnit.FEET => 1,
                LengthUnit.INCHES => 1.0 / 12,
                LengthUnit.YARDS => 3,
                LengthUnit.CENTIMETERS => 0.0328084,
                _ => throw new ArgumentException("Invalid unit")
            };
        }

        /// <summary>
        /// Convert value to base unit (FEET)
        /// </summary>
        public static double ConvertToBaseUnit(this LengthUnit unit, double value)
        {
            return value * unit.GetConversionFactor();
        }

        /// <summary>
        /// Convert value from base unit (FEET)
        /// </summary>
        public static double ConvertFromBaseUnit(this LengthUnit unit, double baseValue)
        {
            return baseValue / unit.GetConversionFactor();
        }
    }
}