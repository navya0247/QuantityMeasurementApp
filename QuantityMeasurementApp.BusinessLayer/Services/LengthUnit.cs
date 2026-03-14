using System;
using QuantityMeasurementApp.ModelLayer.Models;

/// <summary>
/// Service class containing logic for length unit conversions.
/// Converts values to and from the base unit FEET.
/// </summary>

namespace QuantityMeasurementApp.BusinessLayer.Services
{
    public static class LengthUnit
    {
        public static double GetConversionFactor(LengthEnum unit)
        {
            return unit switch
            {
                LengthEnum.FEET => 1,
                LengthEnum.INCHES => 1.0 / 12,
                LengthEnum.YARDS => 3,
                LengthEnum.CENTIMETERS => 0.0328084,
                _ => throw new ArgumentException("Invalid length unit")
            };
        }

        public static double ConvertToBaseUnit(LengthEnum unit, double value)
        {
            return value * GetConversionFactor(unit);
        }

        public static double ConvertFromBaseUnit(LengthEnum unit, double baseValue)
        {
            return baseValue / GetConversionFactor(unit);
        }
    }
}