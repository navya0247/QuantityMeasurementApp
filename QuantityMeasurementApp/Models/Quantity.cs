using System;

namespace QuantityMeasurementApp.Models
{
    /// <summary>
    /// UC10
    /// Generic Quantity class supporting multiple measurement categories
    /// such as Length, Weight, and Volume.
    /// 
    /// UC11 extends this design by adding VolumeUnit without modifying
    /// the core logic of the class.
    /// </summary>
    public class Quantity<U>
    {
        /// <summary>
        /// Numerical value of the measurement.
        /// </summary>
        public double Value { get; }

        /// <summary>
        /// Unit type (LengthUnit, WeightUnit, VolumeUnit).
        /// </summary>
        public U Unit { get; }

        /// <summary>
        /// Constructor to create a Quantity object.
        /// </summary>
        public Quantity(double value, U unit)
        {
            Value = value;
            Unit = unit;
        }

        /// <summary>
        /// Converts the current quantity to a target unit.
        /// </summary>
        public Quantity<U> ConvertTo(U targetUnit)
        {
            double baseValue = ConvertToBase(Value, Unit);
            double targetValue = ConvertFromBase(baseValue, targetUnit);

            return new Quantity<U>(targetValue, targetUnit);
        }

        /// <summary>
        /// Adds two quantities and returns the result in the specified target unit.
        /// </summary>
        public Quantity<U> Add(Quantity<U> other, U targetUnit)
        {
            double base1 = ConvertToBase(Value, Unit);
            double base2 = ConvertToBase(other.Value, other.Unit);

            double sumBase = base1 + base2;

            double result = ConvertFromBase(sumBase, targetUnit);

            return new Quantity<U>(result, targetUnit);
        }

        /// <summary>
        /// Checks equality of two quantities by converting both
        /// to their respective base units.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj is not Quantity<U> other)
                return false;

            double base1 = ConvertToBase(Value, Unit);
            double base2 = ConvertToBase(other.Value, other.Unit);

            return Math.Abs(base1 - base2) < 0.0001;
        }

        /// <summary>
        /// Generates hash code based on value and unit.
        /// </summary>
        public override int GetHashCode()
        {
            return HashCode.Combine(Value, Unit);
        }

        /// <summary>
        /// Converts a value to the base unit depending on measurement category.
        /// </summary>
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

        /// <summary>
        /// Converts a base unit value to the target unit.
        /// </summary>
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

        /// <summary>
        /// Returns readable representation of quantity.
        /// </summary>
        public override string ToString()
        {
            return $"{Value} {Unit}";
        }
    }
}