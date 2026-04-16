using System;
using QmaService.Enums;

namespace QmaService.Services
{
    public static class LengthUnit
    {
        public static double GetConversionFactor(LengthEnum unit) => unit switch
        {
            LengthEnum.FEET        => 1,
            LengthEnum.INCHES      => 1.0 / 12,
            LengthEnum.YARDS       => 3,
            LengthEnum.CENTIMETERS => 0.0328084,
            _                      => throw new ArgumentException("Invalid length unit")
        };

        public static double ConvertToBaseUnit(LengthEnum unit, double value)
            => value * GetConversionFactor(unit);

        public static double ConvertFromBaseUnit(LengthEnum unit, double baseValue)
            => baseValue / GetConversionFactor(unit);
    }

    public static class WeightUnit
    {
        public static double GetConversionFactor(WeightEnum unit) => unit switch
        {
            WeightEnum.KILOGRAM => 1,
            WeightEnum.GRAM     => 0.001,
            WeightEnum.POUND    => 0.453592,
            _                   => throw new ArgumentException("Invalid weight unit")
        };

        public static double ConvertToBaseUnit(WeightEnum unit, double value)
            => value * GetConversionFactor(unit);

        public static double ConvertFromBaseUnit(WeightEnum unit, double baseValue)
            => baseValue / GetConversionFactor(unit);
    }

    public static class VolumeUnit
    {
        public static double ConvertToBaseUnit(VolumeEnum unit, double value) => unit switch
        {
            VolumeEnum.LITRE      => value,
            VolumeEnum.MILLILITRE => value * 0.001,
            VolumeEnum.GALLON     => value * 3.78541,
            _                     => throw new Exception("Invalid volume unit")
        };

        public static double ConvertFromBaseUnit(VolumeEnum unit, double baseValue) => unit switch
        {
            VolumeEnum.LITRE      => baseValue,
            VolumeEnum.MILLILITRE => baseValue * 1000,
            VolumeEnum.GALLON     => baseValue / 3.78541,
            _                     => throw new Exception("Invalid volume unit")
        };
    }

    public static class TemperatureUnit
    {
        public static double ConvertToBaseUnit(TemperatureEnum unit, double value) => unit switch
        {
            TemperatureEnum.CELSIUS    => value,
            TemperatureEnum.FAHRENHEIT => (value - 32) * 5 / 9,
            TemperatureEnum.KELVIN     => value - 273.15,
            _                          => throw new InvalidOperationException()
        };

        public static double ConvertFromBaseUnit(TemperatureEnum unit, double baseValue) => unit switch
        {
            TemperatureEnum.CELSIUS    => baseValue,
            TemperatureEnum.FAHRENHEIT => (baseValue * 9 / 5) + 32,
            TemperatureEnum.KELVIN     => baseValue + 273.15,
            _                          => throw new InvalidOperationException()
        };

        public static void ValidateOperationSupport(string operation)
            => throw new NotSupportedException(
                $"Temperature does not support arithmetic operation: {operation}");
    }

    /// <summary>
    /// Generic Quantity class supporting length, weight, volume, and temperature.
    /// </summary>
    public class Quantity<U>
    {
        public double Value { get; }
        public U      Unit  { get; }

        public Quantity(double value, U unit)
        {
            if (unit == null) throw new ArgumentException("Unit cannot be null");
            Value = value;
            Unit  = unit;
        }

        public Quantity<U> ConvertTo(U targetUnit)
        {
            double baseValue  = ConvertToBase(Value, Unit);
            double converted  = ConvertFromBase(baseValue, targetUnit);
            return new Quantity<U>(converted, targetUnit);
        }

        public Quantity<U> Add(Quantity<U> other, U targetUnit)
        {
            ValidateArithmetic(Unit, "ADD");
            double base1  = ConvertToBase(Value, Unit);
            double base2  = ConvertToBase(other.Value, other.Unit);
            double result = ConvertFromBase(base1 + base2, targetUnit);
            return new Quantity<U>(result, targetUnit);
        }

        public Quantity<U> Subtract(Quantity<U> other)
        {
            ValidateArithmetic(Unit, "SUBTRACT");
            double base1  = ConvertToBase(Value, Unit);
            double base2  = ConvertToBase(other.Value, other.Unit);
            double result = ConvertFromBase(base1 - base2, Unit);
            return new Quantity<U>(result, Unit);
        }

        public double Divide(Quantity<U> other)
        {
            ValidateArithmetic(Unit, "DIVIDE");
            double base1 = ConvertToBase(Value, Unit);
            double base2 = ConvertToBase(other.Value, other.Unit);
            if (base2 == 0) throw new ArithmeticException("Cannot divide by zero");
            return base1 / base2;
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Quantity<U> other) return false;
            double base1 = ConvertToBase(Value, Unit);
            double base2 = ConvertToBase(other.Value, other.Unit);
            return Math.Abs(base1 - base2) < 0.0001;
        }

        public override int    GetHashCode() => HashCode.Combine(Value, Unit);
        public override string ToString()    => $"{Value} {Unit}";

        private static double ConvertToBase(double value, U unit)
        {
            if (unit is LengthEnum      lu) return LengthUnit.ConvertToBaseUnit(lu, value);
            if (unit is WeightEnum      wu) return WeightUnit.ConvertToBaseUnit(wu, value);
            if (unit is VolumeEnum      vu) return VolumeUnit.ConvertToBaseUnit(vu, value);
            if (unit is TemperatureEnum tu) return TemperatureUnit.ConvertToBaseUnit(tu, value);
            throw new InvalidOperationException("Unsupported unit type");
        }

        private static double ConvertFromBase(double baseValue, U unit)
        {
            if (unit is LengthEnum      lu) return LengthUnit.ConvertFromBaseUnit(lu, baseValue);
            if (unit is WeightEnum      wu) return WeightUnit.ConvertFromBaseUnit(wu, baseValue);
            if (unit is VolumeEnum      vu) return VolumeUnit.ConvertFromBaseUnit(vu, baseValue);
            if (unit is TemperatureEnum tu) return TemperatureUnit.ConvertFromBaseUnit(tu, baseValue);
            throw new InvalidOperationException("Unsupported unit type");
        }

        private static void ValidateArithmetic(U unit, string operation)
        {
            if (unit is TemperatureEnum) TemperatureUnit.ValidateOperationSupport(operation);
        }
    }
}
