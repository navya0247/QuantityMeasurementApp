using System;

namespace QuantityMeasurementApp.Models
{
    public class QuantityLength
    {
        private readonly double value;
        private readonly LengthUnit unit;


        // Public read-only properties
        public double Value => value;
        public LengthUnit Unit => unit;

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

        // ================= UC5 =================
        // Generic Conversion Method
        public static double Convert(double value, LengthUnit from, LengthUnit to)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
                throw new ArgumentException("Invalid numeric value");

            double valueInFeet = value * from.ToFeetFactor();
            return valueInFeet / to.ToFeetFactor();
        }

        // ================= UC6 =================
        // ADDITION METHOD
        public static QuantityLength Add(QuantityLength a, QuantityLength b)
        {
            if (a == null || b == null)
                throw new ArgumentException("Quantity cannot be null");

            // Convert both to feet
            double sumFeet = a.ConvertToFeet() + b.ConvertToFeet();

            // Convert result back to FIRST operand unit
            double resultValue = sumFeet / a.unit.ToFeetFactor();

            return new QuantityLength(resultValue, a.unit);
        }

        // Overloaded addition 
        public static QuantityLength Add(double v1, LengthUnit u1, double v2, LengthUnit u2)
        {
            return Add(new QuantityLength(v1, u1),
                       new QuantityLength(v2, u2));
        }

        // ================= EQUALITY =================
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
