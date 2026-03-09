using System;

namespace QuantityMeasurementApp.Models
{
    public class QuantityWeight
    {
        private readonly double value;
        private readonly WeightUnit unit;

        public double Value => value;
        public WeightUnit Unit => unit;

        public QuantityWeight(double value, WeightUnit unit)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
                throw new ArgumentException("Invalid numeric value");

            this.value = value;
            this.unit = unit;
        }

        private double ConvertToKilogram()
        {
            return unit.ConvertToBaseUnit(value);
        }

        // Conversion
        public QuantityWeight ConvertTo(WeightUnit targetUnit)
        {
            double baseValue = unit.ConvertToBaseUnit(value);
            double converted = targetUnit.ConvertFromBaseUnit(baseValue);

            return new QuantityWeight(converted, targetUnit);
        }

        // Addition (UC6 )
        public static QuantityWeight Add(QuantityWeight a, QuantityWeight b)
        {
            return Add(a, b, a.unit);
        }

        // Addition (UC7 )
        public static QuantityWeight Add(
            QuantityWeight a,
            QuantityWeight b,
            WeightUnit targetUnit)
        {
            if (a == null || b == null)
                throw new ArgumentException("Weight cannot be null");

            double sumKg = a.ConvertToKilogram() + b.ConvertToKilogram();
            double result = targetUnit.ConvertFromBaseUnit(sumKg);

            return new QuantityWeight(result, targetUnit);
        }

        public override bool Equals(object? obj)
        {
            if (this == obj) return true;
            if (obj == null || GetType() != obj.GetType()) return false;

            QuantityWeight other = (QuantityWeight)obj;

            double epsilon = 0.0001;

            return Math.Abs(this.ConvertToKilogram() - other.ConvertToKilogram()) < epsilon;
        }

        public override int GetHashCode()
        {
            return ConvertToKilogram().GetHashCode();
        }
    }
}