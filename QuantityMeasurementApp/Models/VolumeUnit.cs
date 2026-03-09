using System;

namespace QuantityMeasurementApp.Models
{
    /// <summary>
    /// UC11
    /// VolumeUnit enum represents different volume measurement units.
    /// Base unit: LITRE
    /// Supported units:
    /// LITRE (base unit)
    /// MILLILITRE (1 L = 1000 mL)
    /// GALLON (1 gallon ≈ 3.78541 L)
    /// </summary>
    public enum VolumeUnit
    {
        LITRE,
        MILLILITRE,
        GALLON
    }

    /// <summary>
    /// Extension methods for VolumeUnit.
    /// Handles conversion to and from the base unit (LITRE).
    /// </summary>
    public static class VolumeUnitExtensions
    {
        /// <summary>
        /// Converts a value from the specified unit to the base unit (LITRE).
        /// </summary>
        public static double ConvertToBaseUnit(this VolumeUnit unit, double value)
        {
            return unit switch
            {
                VolumeUnit.LITRE => value,
                VolumeUnit.MILLILITRE => value * 0.001,
                VolumeUnit.GALLON => value * 3.78541,
                _ => throw new Exception("Invalid volume unit")
            };
        }

        /// <summary>
        /// Converts a value from base unit (LITRE) to the specified unit.
        /// </summary>
        public static double ConvertFromBaseUnit(this VolumeUnit unit, double baseValue)
        {
            return unit switch
            {
                VolumeUnit.LITRE => baseValue,
                VolumeUnit.MILLILITRE => baseValue * 1000,
                VolumeUnit.GALLON => baseValue / 3.78541,
                _ => throw new Exception("Invalid volume unit")
            };
        }
    }
}