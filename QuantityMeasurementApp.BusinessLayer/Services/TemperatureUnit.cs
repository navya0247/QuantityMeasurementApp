using System;
using QuantityMeasurementApp.ModelLayer.Models;

/// <summary>
/// Service class for temperature conversion logic.
/// Base unit used is CELSIUS.
/// Arithmetic operations are not supported.
/// </summary>

namespace QuantityMeasurementApp.BusinessLayer.Services
{
    public static class TemperatureUnit
    {
        public static double ConvertToBaseUnit(TemperatureEnum unit, double value)
        {
            return unit switch
            {
                TemperatureEnum.CELSIUS => value,
                TemperatureEnum.FAHRENHEIT => (value - 32) * 5 / 9,
                TemperatureEnum.KELVIN => value - 273.15,
                _ => throw new InvalidOperationException()
            };
        }

        public static double ConvertFromBaseUnit(TemperatureEnum unit, double baseValue)
        {
            return unit switch
            {
                TemperatureEnum.CELSIUS => baseValue,
                TemperatureEnum.FAHRENHEIT => (baseValue * 9 / 5) + 32,
                TemperatureEnum.KELVIN => baseValue + 273.15,
                _ => throw new InvalidOperationException()
            };
        }

        public static void ValidateOperationSupport(string operation)
        {
            throw new NotSupportedException(
                $"Temperature does not support arithmetic operation: {operation}");
        }
    }
}