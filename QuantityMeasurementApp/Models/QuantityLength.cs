using System;

namespace QuantityMeasurementApp.Models
{
    // Generic class representing any length quantity
    public class QuantityLength
    {
        private readonly double value;
        private readonly LengthUnit unit;

        // Constructor
        public QuantityLength(double value, LengthUnit unit)
        {
            this.value = value;
            this.unit = unit;
        }

        // Convert any unit to base unit (feet)
        private double ConvertToFeet()
        {
            if (unit == LengthUnit.FEET)
                return value;

            if (unit == LengthUnit.INCH)
                return value / 12.0;

            return 0;
        }

        // Override equals method for cross-unit comparison
        public override bool Equals(object? obj)
        {
            if (this == obj)
                return true;

            if (obj == null || GetType() != obj.GetType())
                return false;

            QuantityLength other = (QuantityLength)obj;

           return this.ConvertToFeet().CompareTo(other.ConvertToFeet()) == 0;
        }

        public override int GetHashCode()
        {
            return ConvertToFeet().GetHashCode();
        }
    }
}