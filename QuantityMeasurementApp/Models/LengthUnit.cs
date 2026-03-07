using System;

namespace QuantityMeasurementApp.Models
{
    // UC8: Standalone enum with conversion responsibility
    public enum LengthUnit
    {
        FEET,
        INCHES,
        YARDS,
        CENTIMETERS
    }

    public static class LengthUnitExtensions
    {
        // Convert value from this unit to base unit (FEET)
        public static double ConvertToBaseUnit(this LengthUnit unit, double value)
        {
            return unit switch
            {
                LengthUnit.FEET => value,
                LengthUnit.INCHES => value / 12.0,
                LengthUnit.YARDS => value * 3.0,
                LengthUnit.CENTIMETERS => value * 0.0328084,
                _ => throw new ArgumentException("Invalid unit")
            };
        }

        // Convert value from base unit (FEET) to target unit
        public static double ConvertFromBaseUnit(this LengthUnit unit, double baseValue)
        {
            return unit switch
            {
                LengthUnit.FEET => baseValue,
                LengthUnit.INCHES => baseValue * 12.0,
                LengthUnit.YARDS => baseValue / 3.0,
                LengthUnit.CENTIMETERS => baseValue / 0.0328084,
                _ => throw new ArgumentException("Invalid unit")
            };
        }
    }
}