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

        // Convert any unit to base unit (FEET)
        private double ConvertToFeet()
        {
            // Feet - base unit
            if (unit == LengthUnit.FEET)
                return value;

            // Inches - convert to feet
            if (unit == LengthUnit.INCHES)
                return value / 12.0;

            // Yards - convert to feet
            if (unit == LengthUnit.YARDS)
                return value * 3.0;   // 1 yard = 3 feet

            // Centimeters , convert to inches , then to feet
            if (unit == LengthUnit.CENTIMETERS)
                return (value * 0.393701) / 12.0;

            return 0;
        }

        // Override equals method for cross-unit comparison
        public override bool Equals(object? obj)
        {
            // Same reference check
            if (this == obj)
                return true;

            // Null or different type check
            if (obj == null || GetType() != obj.GetType())
                return false;

            QuantityLength other = (QuantityLength)obj;

            // Compare after converting to base unit
            return this.ConvertToFeet().CompareTo(other.ConvertToFeet()) == 0;
        }

        // Override hashcode
        public override int GetHashCode()
        {
            return ConvertToFeet().GetHashCode();
        }
    }
}