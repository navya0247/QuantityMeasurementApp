using System;

namespace QuantityMeasurementApp.Models
{
    public class Feet
    {
        // Private readonly field for immutability
        private readonly double value;

        // Constructor to initialize value
        public Feet(double value)
        {
            this.value = value;
        }

        // Getter method
        public double GetValue()
        {
            return value;
        }

        // Override equals method
        public override bool Equals(object obj)
        {
            // Check same reference
            if (this == obj)
                return true;

            // Check null or different type
            if (obj == null || GetType() != obj.GetType())
                return false;

            // Cast safely
            Feet other = (Feet)obj;

          
            return this.value.CompareTo(other.value) == 0;
        }

        // Override hashcode
        public override int GetHashCode()
        {
            return value.GetHashCode();
        }
    }
}