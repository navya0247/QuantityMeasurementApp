using System;

namespace QuantityMeasurementApp.Models
{
    public class QuantityLength
    {
        private readonly double value;
        private readonly LengthUnit unit;

        public double Value => value;
        public LengthUnit Unit => unit;

        public QuantityLength(double value, LengthUnit unit)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
                throw new ArgumentException("Invalid numeric value");

            this.value = value;
            this.unit = unit;
        }

        // UC8: Delegate conversion to unit
        private double ConvertToFeet()
        {
            return unit.ConvertToBaseUnit(value);
        }

        // UC5 Conversion
        public static double Convert(double value, LengthUnit from, LengthUnit to)
        {
            double baseValue = from.ConvertToBaseUnit(value);
            return to.ConvertFromBaseUnit(baseValue);
        }

        // UC6
        public static QuantityLength Add(QuantityLength a, QuantityLength b)
        {
            return Add(a, b, a.unit);
        }

        public static QuantityLength Add(double v1, LengthUnit u1, double v2, LengthUnit u2)
        {
            return Add(new QuantityLength(v1, u1),
                       new QuantityLength(v2, u2));
        }

        // UC7
        public static QuantityLength Add(
            QuantityLength a,
            QuantityLength b,
            LengthUnit targetUnit)
        {
            if (a == null || b == null)
                throw new ArgumentException("Quantity cannot be null");

            double sumFeet = a.ConvertToFeet() + b.ConvertToFeet();

            double result = targetUnit.ConvertFromBaseUnit(sumFeet);

            return new QuantityLength(result, targetUnit);
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