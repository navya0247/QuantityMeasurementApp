using System;

namespace QuantityMeasurementApp.Models
{
    public enum WeightUnit
    {
        KILOGRAM,
        GRAM,
        POUND
    }

    public static class WeightUnitExtensions
    {
        // Convert to base unit (KILOGRAM)
        public static double ConvertToBaseUnit(this WeightUnit unit, double value)
        {
            return unit switch
            {
                WeightUnit.KILOGRAM => value,
                WeightUnit.GRAM => value * 0.001,
                WeightUnit.POUND => value * 0.453592,
                _ => throw new ArgumentException("Invalid weight unit")
            };
        }

        // Convert from base unit
        public static double ConvertFromBaseUnit(this WeightUnit unit, double baseValue)
        {
            return unit switch
            {
                WeightUnit.KILOGRAM => baseValue,
                WeightUnit.GRAM => baseValue / 0.001,
                WeightUnit.POUND => baseValue / 0.453592,
                _ => throw new ArgumentException("Invalid weight unit")
            };
        }
    }
}