using System;
using QuantityMeasurementApp.ModelLayer.Models;

/// <summary>
/// Service class responsible for weight unit conversions.
/// Converts values to and from the base unit KILOGRAM.
/// </summary>

namespace QuantityMeasurementApp.BusinessLayer.Services
{
    public static class WeightUnit
    {
        public static double GetConversionFactor(WeightEnum unit)
        {
            return unit switch
            {
                WeightEnum.KILOGRAM => 1,
                WeightEnum.GRAM => 0.001,
                WeightEnum.POUND => 0.453592,
                _ => throw new ArgumentException("Invalid weight unit")
            };
        }

        public static double ConvertToBaseUnit(WeightEnum unit, double value)
        {
            return value * GetConversionFactor(unit);
        }

        public static double ConvertFromBaseUnit(WeightEnum unit, double baseValue)
        {
            return baseValue / GetConversionFactor(unit);
        }

        public static string GetUnitName(WeightEnum unit)
        {
            return unit.ToString();
        }
    }
}