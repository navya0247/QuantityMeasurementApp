using System;

namespace QuantityMeasurementApp.Models
{
    /// <summary>
    /// UC10
    /// Generic Quantity class supporting different measurement units
    /// like LengthUnit and WeightUnit.
    /// </summary>
    public class Quantity<U>
    {
        public double Value { get; }
        public U Unit { get; }

        public Quantity(double value, U unit)
        {
            Value = value;
            Unit = unit;
        }

        // Convert quantity to another unit
        public Quantity<U> ConvertTo(U targetUnit)
        {
            double baseValue = ConvertToBase(Value, Unit);
            double targetValue = ConvertFromBase(baseValue, targetUnit);

            return new Quantity<U>(targetValue, targetUnit);
        }

        // Add two quantities
        public Quantity<U> Add(Quantity<U> other, U targetUnit)
        {
            double base1 = ConvertToBase(Value, Unit);
            double base2 = ConvertToBase(other.Value, other.Unit);

            double sumBase = base1 + base2;

            double result = ConvertFromBase(sumBase, targetUnit);

            return new Quantity<U>(result, targetUnit);
        }

        public override bool Equals(object obj)
        {
            if (obj is not Quantity<U> other)
                return false;

            double base1 = ConvertToBase(Value, Unit);
            double base2 = ConvertToBase(other.Value, other.Unit);

            return Math.Abs(base1 - base2) < 0.0001;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Value, Unit);
        }

        // Helper methods to handle both unit types
        private static double ConvertToBase(double value, U unit)
        {
            if (unit is LengthUnit lu)
                return lu.ConvertToBaseUnit(value);

            if (unit is WeightUnit wu)
                return wu.ConvertToBaseUnit(value);

            throw new InvalidOperationException("Unsupported unit type");
        }

        private static double ConvertFromBase(double baseValue, U unit)
        {
            if (unit is LengthUnit lu)
                return lu.ConvertFromBaseUnit(baseValue);

            if (unit is WeightUnit wu)
                return wu.ConvertFromBaseUnit(baseValue);

            throw new InvalidOperationException("Unsupported unit type");
        }

        public override string ToString()
        {
            return $"{Value} {Unit}";
        }
    }
}