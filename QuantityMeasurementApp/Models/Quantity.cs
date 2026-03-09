using System;

namespace QuantityMeasurementApp.Models
{
    /// <summary>
    /// Generic Quantity class supporting Length, Weight and Volume measurements.
    /// UC12 adds subtraction and division operations.
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
            double converted = ConvertFromBase(baseValue, targetUnit);

            return new Quantity<U>(converted, targetUnit);
        }

        // Addition
        public Quantity<U> Add(Quantity<U> other, U targetUnit)
        {
            double base1 = ConvertToBase(Value, Unit);
            double base2 = ConvertToBase(other.Value, other.Unit);

            double resultBase = base1 + base2;
            double result = ConvertFromBase(resultBase, targetUnit);

            return new Quantity<U>(result, targetUnit);
        }

        // UC12 Subtraction
        public Quantity<U> Subtract(Quantity<U> other)
        {
            return Subtract(other, Unit);
        }

        public Quantity<U> Subtract(Quantity<U> other, U targetUnit)
        {
            if (other == null)
                throw new ArgumentException("Quantity cannot be null");

            double base1 = ConvertToBase(Value, Unit);
            double base2 = ConvertToBase(other.Value, other.Unit);

            double resultBase = base1 - base2;
            double result = ConvertFromBase(resultBase, targetUnit);

            return new Quantity<U>(result, targetUnit);
        }

        // UC12 Division
        public double Divide(Quantity<U> other)
        {
            if (other == null)
                throw new ArgumentException("Quantity cannot be null");

            double base1 = ConvertToBase(Value, Unit);
            double base2 = ConvertToBase(other.Value, other.Unit);

            if (base2 == 0)
                throw new ArithmeticException("Cannot divide by zero");

            return base1 / base2;
        }

        // Equality
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

        public override string ToString()
        {
            return $"{Value} {Unit}";
        }

        // Convert value to base unit
        private static double ConvertToBase(double value, U unit)
        {
            if (unit is LengthUnit lu)
                return lu.ConvertToBaseUnit(value);

            if (unit is WeightUnit wu)
                return wu.ConvertToBaseUnit(value);

            if (unit is VolumeUnit vu)
                return vu.ConvertToBaseUnit(value);

            throw new InvalidOperationException("Unsupported unit type");
        }

        // Convert base value to target unit
        private static double ConvertFromBase(double baseValue, U unit)
        {
            if (unit is LengthUnit lu)
                return lu.ConvertFromBaseUnit(baseValue);

            if (unit is WeightUnit wu)
                return wu.ConvertFromBaseUnit(baseValue);

            if (unit is VolumeUnit vu)
                return vu.ConvertFromBaseUnit(baseValue);

            throw new InvalidOperationException("Unsupported unit type");
        }
    }
}