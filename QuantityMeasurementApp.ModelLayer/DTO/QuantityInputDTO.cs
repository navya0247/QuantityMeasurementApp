using System.ComponentModel.DataAnnotations;

namespace QuantityMeasurementApp.ModelLayer.DTO
{
    /// <summary>
    /// Standard input for COMPARE, CONVERT, DIVIDE operations.
    /// </summary>
    /// <example>
    /// {
    ///   "thisQuantity": { "value": 1.0,  "unit": "FEET",   "measurementType": "LengthUnit" },
    ///   "thatQuantity": { "value": 12.0, "unit": "INCHES", "measurementType": "LengthUnit" }
    /// }
    /// </example>
    public class QuantityInputDTO
    {
        [Required(ErrorMessage = "First quantity is required.")]
        public QuantityRequestDTO ThisQuantity { get; set; } = new QuantityRequestDTO
        {
            Value           = 1.0,
            Unit            = "FEET",
            MeasurementType = "LengthUnit"
        };

        [Required(ErrorMessage = "Second quantity is required.")]
        public QuantityRequestDTO ThatQuantity { get; set; } = new QuantityRequestDTO
        {
            Value           = 12.0,
            Unit            = "INCHES",
            MeasurementType = "LengthUnit"
        };
    }

    /// <summary>
    /// Input for ADD and SUBTRACT operations — includes optional targetUnit.
    /// </summary>
    /// <example>
    /// {
    ///   "thisQuantity": { "value": 1.0,  "unit": "FEET",   "measurementType": "LengthUnit" },
    ///   "thatQuantity": { "value": 12.0, "unit": "INCHES", "measurementType": "LengthUnit" },
    ///   "targetUnit":   { "value": 0.0,  "unit": "YARDS",  "measurementType": "LengthUnit" }
    /// }
    /// </example>
    public class ArithmeticInputDTO
    {
        [Required(ErrorMessage = "First quantity is required.")]
        public QuantityRequestDTO ThisQuantity { get; set; } = new QuantityRequestDTO
        {
            Value           = 1.0,
            Unit            = "FEET",
            MeasurementType = "LengthUnit"
        };

        [Required(ErrorMessage = "Second quantity is required.")]
        public QuantityRequestDTO ThatQuantity { get; set; } = new QuantityRequestDTO
        {
            Value           = 12.0,
            Unit            = "INCHES",
            MeasurementType = "LengthUnit"
        };

        /// <summary>
        /// Optional — specifies the unit for the result.
        /// If not provided, result uses first quantity unit.
        /// </summary>
        public QuantityRequestDTO? TargetUnit { get; set; }
    }

    /// <summary>
    /// A single quantity with value, unit, and measurement type.
    ///
    /// measurementType options:
    ///   LengthUnit      — FEET | INCHES | YARDS | CENTIMETERS
    ///   WeightUnit      — KILOGRAM | GRAM | POUND
    ///   VolumeUnit      — LITRE | MILLILITRE | GALLON
    ///   TemperatureUnit — CELSIUS | FAHRENHEIT | KELVIN
    ///
    /// Note: Lowercase also works — feet, gram, celsius all accepted.
    /// </summary>
    public class QuantityRequestDTO
    {
        /// <summary>Numeric value. Example: 1.0</summary>
        [Required]
        public double Value { get; set; } = 1.0;

        /// <summary>Unit name. Example: FEET, KILOGRAM, LITRE, CELSIUS</summary>
        [Required(ErrorMessage = "Unit is required.")]
        public string Unit { get; set; } = "FEET";

        /// <summary>Measurement category. Example: LengthUnit, WeightUnit, VolumeUnit, TemperatureUnit</summary>
        [Required(ErrorMessage = "MeasurementType is required.")]
        public string MeasurementType { get; set; } = "LengthUnit";
    }
}
