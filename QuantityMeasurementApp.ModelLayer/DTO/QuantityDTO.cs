namespace QuantityMeasurementApp.ModelLayer.DTO
{
    /// <summary>
    /// DTO used to transfer quantity data
    /// between controller and service layers.
    /// </summary>
    public class QuantityDTO
    {
        public double Value { get; set; }

        public object Unit { get; set; }

        public QuantityDTO(double value, object unit)
        {
            Value = value;
            Unit = unit;
        }
    }
}