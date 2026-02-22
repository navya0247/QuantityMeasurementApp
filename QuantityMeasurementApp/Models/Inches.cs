using System;

namespace QuantityMeasurementApp.Models
{
    // Class representing Inches measurement
    public class Inches
    {
        // Private readonly field to store value safely
        private readonly double value;

        // Constructor to initialize inches value
        public Inches(double value)
        {
            this.value = value;
        }

        // Override Equals method for value comparison
        public override bool Equals(object obj)
        {
            // Check same reference
            if (this == obj)
                return true;

            // Check null or different type
            if (obj == null || GetType() != obj.GetType())
                return false;

            // Safe casting
            Inches other = (Inches)obj;

            // Compare double values safely
            return this.value.CompareTo(other.value) == 0;
        }

        // Override hashcode
        public override int GetHashCode()
        {
            return value.GetHashCode();
        }
    }
}