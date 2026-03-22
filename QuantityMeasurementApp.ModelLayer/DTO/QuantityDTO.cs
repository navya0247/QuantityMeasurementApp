namespace QuantityMeasurementApp.ModelLayer.DTO
{
    /// <summary>Transfers a single quantity value and unit between layers.</summary>
    public class QuantityDTO
    {
        public double Value { get; set; }
        public object Unit  { get; set; }

        public QuantityDTO(double value, object unit)
        {
            Value = value;
            Unit  = unit;
        }
    }
}
