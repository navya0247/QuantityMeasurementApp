using System;

namespace QuantityMeasurementApp.Models
{
    public class QuantityLength
    {
        private readonly double value;
        private readonly LengthUnit unit;

        public QuantityLength(double value, LengthUnit unit)
        {
            this.value = value;
            this.unit = unit;
        }

        // Convert to FEET using enum factor
        private double ConvertToFeet()
        {
            return value * unit.ToFeetFactor();
        }

        // UC5 — GENERIC CONVERSION METHOD
        public static double Convert(double value, LengthUnit from, LengthUnit to)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
                throw new ArgumentException("Invalid numeric value");

            double valueInFeet = value * from.ToFeetFactor();
            return valueInFeet / to.ToFeetFactor();
        }

        public override bool Equals(object? obj)
        {
            if (this == obj) return true;
            if (obj == null || GetType() != obj.GetType()) return false;

            QuantityLength other = (QuantityLength)obj;

            double epsilon = 0.0001;
            return Math.Abs(this.ConvertToFeet() - other.ConvertToFeet()) < epsilon;
        }

        public override int GetHashCode()
        {
            return ConvertToFeet().GetHashCode();
        }
    }
}