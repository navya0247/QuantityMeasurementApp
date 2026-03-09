using System;

namespace QuantityMeasurementApp.Models
{
    /// <summary>
    /// Generic Quantity class supporting Length, Weight and Volume measurements.
    /// UC13 refactors arithmetic operations using centralized helper logic
    /// to enforce DRY principle.
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

        /// <summary>
        /// Arithmetic operation types used by centralized helper
        /// </summary>
        private enum ArithmeticOperation
        {
            ADD,
            SUBTRACT,
            DIVIDE
        }

        // Convert quantity to another unit
        public Quantity<U> ConvertTo(U targetUnit)
        {
            double baseValue = ConvertToBase(Value, Unit);
            double converted = ConvertFromBase(baseValue, targetUnit);

            return new Quantity<U>(converted, targetUnit);
        }

        // ---------------- ADD ----------------

        public Quantity<U> Add(Quantity<U> other, U targetUnit)
        {
            ValidateArithmeticOperands(other, targetUnit, true);

            double baseResult = PerformBaseArithmetic(other, ArithmeticOperation.ADD);

            double result = ConvertFromBase(baseResult, targetUnit);

            return new Quantity<U>(result, targetUnit);
        }

        // ---------------- SUBTRACT ----------------

        public Quantity<U> Subtract(Quantity<U> other)
        {
            return Subtract(other, Unit);
        }

        public Quantity<U> Subtract(Quantity<U> other, U targetUnit)
        {
            ValidateArithmeticOperands(other, targetUnit, true);

            double baseResult = PerformBaseArithmetic(other, ArithmeticOperation.SUBTRACT);

            double result = ConvertFromBase(baseResult, targetUnit);

            return new Quantity<U>(result, targetUnit);
        }

        // ---------------- DIVIDE ----------------

        public double Divide(Quantity<U> other)
        {
            ValidateArithmeticOperands(other, default, false);

            return PerformBaseArithmetic(other, ArithmeticOperation.DIVIDE);
        }

        /// <summary>
        /// Centralized validation for arithmetic operations
        /// </summary>
        private void ValidateArithmeticOperands(Quantity<U> other, U targetUnit, bool targetUnitRequired)
        {
            if (other == null)
                throw new ArgumentException("Quantity cannot be null");

            if (targetUnitRequired && targetUnit == null)
                throw new ArgumentException("Target unit cannot be null");

            if (double.IsNaN(Value) || double.IsInfinity(Value) ||
                double.IsNaN(other.Value) || double.IsInfinity(other.Value))
                throw new ArgumentException("Values must be finite");

            if (Unit.GetType() != other.Unit.GetType())
                throw new ArgumentException("Different measurement categories");
        }

        /// <summary>
        /// Core centralized arithmetic logic
        /// </summary>
        private double PerformBaseArithmetic(Quantity<U> other, ArithmeticOperation operation)
        {
            double base1 = ConvertToBase(Value, Unit);
            double base2 = ConvertToBase(other.Value, other.Unit);

            return operation switch
            {
                ArithmeticOperation.ADD => base1 + base2,

                ArithmeticOperation.SUBTRACT => base1 - base2,

                ArithmeticOperation.DIVIDE => base2 == 0
                    ? throw new ArithmeticException("Cannot divide by zero")
                    : base1 / base2,

                _ => throw new InvalidOperationException("Unsupported operation")
            };
        }

        // ---------------- EQUALITY ----------------

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