using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Services
{
    public class QuantityMeasurementService
    {
        // Method to check equality of two feet objects
        public bool AreFeetEqual(Feet f1, Feet f2)
        {
            // Return false if any object is null
            if (f1 == null || f2 == null)
                return false;

            // Use Equals method for comparison
            return f1.Equals(f2);
        }
    }
}