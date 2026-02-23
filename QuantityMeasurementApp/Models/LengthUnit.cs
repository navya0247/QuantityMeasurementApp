      

    namespace QuantityMeasurementApp.Models
{

    // Enum storing conversion factor relative to base unit (FEET)
    public enum LengthUnit
    {
        FEET,            
        INCHES,          
        YARDS,
        CENTIMETERS
    }

    public static class LengthUnitExtensions
    {
        // Conversion factor relative to FEET (base unit)
        public static double ToFeetFactor(this LengthUnit unit)
        {
            return unit switch
            {
                LengthUnit.FEET => 1.0,
                LengthUnit.INCHES => 1.0 / 12.0,
                LengthUnit.YARDS => 3.0,
                LengthUnit.CENTIMETERS => 0.0328084,
                _ => throw new ArgumentException("Invalid unit")
            };
        }
    }
}
