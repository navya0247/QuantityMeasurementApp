using System;

namespace QuantityMeasurementApp.Models
{
    /// <summary>
    /// UC14
    /// Temperature measurement units supporting conversion and equality.
    /// Arithmetic operations like add, subtract, divide are not supported.
    ///
    /// Base unit: Celsius
    /// </summary>
    public enum TemperatureUnit
    {
        CELSIUS,
        FAHRENHEIT,
        KELVIN
    }

    public static class TemperatureUnitExtensions
    {
        /// <summary>
        /// Convert temperature to base unit (Celsius)
        /// </summary>
        public static double ConvertToBaseUnit(this TemperatureUnit unit, double value)
        {
            return unit switch
            {
                TemperatureUnit.CELSIUS => value,
                TemperatureUnit.FAHRENHEIT => (value - 32) * 5 / 9,
                TemperatureUnit.KELVIN => value - 273.15,
                _ => throw new InvalidOperationException()
            };
        }

        /// <summary>
        /// Convert base unit (Celsius) to target temperature unit
        /// </summary>
        public static double ConvertFromBaseUnit(this TemperatureUnit unit, double baseValue)
        {
            return unit switch
            {
                TemperatureUnit.CELSIUS => baseValue,
                TemperatureUnit.FAHRENHEIT => (baseValue * 9 / 5) + 32,
                TemperatureUnit.KELVIN => baseValue + 273.15,
                _ => throw new InvalidOperationException()
            };
        }

        /// <summary>
        /// Temperature does NOT support arithmetic operations.
        /// </summary>
        public static void ValidateOperationSupport(this TemperatureUnit unit, string operation)
        {
            throw new NotSupportedException(
                $"Temperature does not support arithmetic operation: {operation}");
        }
    }
}