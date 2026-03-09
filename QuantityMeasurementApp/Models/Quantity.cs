using System;

namespace QuantityMeasurementApp.Models
{
    /// <summary>
    /// Generic Quantity class supporting multiple measurement categories.
    ///
    /// UC10: Generic quantity implementation
    /// UC11: Volume measurement support
    /// UC12: Add, Subtract, Divide operations
    /// UC13: Centralized arithmetic logic
    /// UC14: Temperature support with restricted arithmetic operations
    /// </summary>
    public class Quantity<U>
    {
        public double Value { get; }
        public U Unit { get; }

        public Quantity(double value, U unit)
        {
            if (unit == null)
                throw new ArgumentException("Unit cannot be null");

            Value = value;
            Unit = unit;
        }

        /// <summary>
        /// Convert quantity to another unit
        /// </summary>
        public Quantity<U> ConvertTo(U targetUnit)
        {
            double baseValue = ConvertToBase(Value, Unit);
            double converted = ConvertFromBase(baseValue, targetUnit);

            return new Quantity<U>(converted, targetUnit);
        }

        /// <summary>
        /// Add quantities
        /// </summary>
        public Quantity<U> Add(Quantity<U> other, U targetUnit)
        {
            ValidateArithmetic(Unit, "ADD");

            double base1 = ConvertToBase(Value, Unit);
            double base2 = ConvertToBase(other.Value, other.Unit);

            double result = ConvertFromBase(base1 + base2, targetUnit);

            return new Quantity<U>(result, targetUnit);
        }

        /// <summary>
        /// Subtract quantities
        /// </summary>
        public Quantity<U> Subtract(Quantity<U> other)
        {
            ValidateArithmetic(Unit, "SUBTRACT");

            double base1 = ConvertToBase(Value, Unit);
            double base2 = ConvertToBase(other.Value, other.Unit);

            double result = ConvertFromBase(base1 - base2, Unit);

            return new Quantity<U>(result, Unit);
        }

        /// <summary>
        /// Divide quantities
        /// </summary>
        public double Divide(Quantity<U> other)
        {
            ValidateArithmetic(Unit, "DIVIDE");

            double base1 = ConvertToBase(Value, Unit);
            double base2 = ConvertToBase(other.Value, other.Unit);

            if (base2 == 0)
                throw new ArithmeticException("Cannot divide by zero");

            return base1 / base2;
        }

        /// <summary>
        /// Equality comparison using base unit
        /// </summary>
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

        private static double ConvertToBase(double value, U unit)
        {
            if (unit is LengthUnit lu)
                return lu.ConvertToBaseUnit(value);

            if (unit is WeightUnit wu)
                return wu.ConvertToBaseUnit(value);

            if (unit is VolumeUnit vu)
                return vu.ConvertToBaseUnit(value);

            if (unit is TemperatureUnit tu)
                return tu.ConvertToBaseUnit(value);

            throw new InvalidOperationException("Unsupported unit type");
        }

        private static double ConvertFromBase(double baseValue, U unit)
        {
            if (unit is LengthUnit lu)
                return lu.ConvertFromBaseUnit(baseValue);

            if (unit is WeightUnit wu)
                return wu.ConvertFromBaseUnit(baseValue);

            if (unit is VolumeUnit vu)
                return vu.ConvertFromBaseUnit(baseValue);

            if (unit is TemperatureUnit tu)
                return tu.ConvertFromBaseUnit(baseValue);

            throw new InvalidOperationException("Unsupported unit type");
        }

        /// <summary>
        /// Validates arithmetic support for specific units.
        /// Temperature throws exception here.
        /// </summary>
        private static void ValidateArithmetic(U unit, string operation)
        {
            if (unit is TemperatureUnit tu)
                tu.ValidateOperationSupport(operation);
        }
    }
}