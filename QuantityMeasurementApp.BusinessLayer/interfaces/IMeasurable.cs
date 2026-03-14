using System;

/// <summary>
/// Domain model representing measurement units and quantity logic.
/// Used by the service layer for calculations.
/// </summary>

namespace QuantityMeasurementApp.ModelLayer.Models
{
    /// <summary>
    /// UC10
    /// Base interface for all measurable units.
    /// Provides methods for converting values to and from a base unit.
    ///
    /// UC14 Update:
    /// Introduces optional arithmetic operation support using default methods.
    /// This allows measurement categories like Temperature to disable
    /// </summary>
    public interface IMeasurable
    {
        /// <summary>
        /// Converts value to base unit of the measurement category.
        /// </summary>
        double ConvertToBaseUnit(double value);

        /// <summary>
        /// Converts value from base unit to this unit.
        /// </summary>
        double ConvertFromBaseUnit(double baseValue);

        /// <summary>
        /// Functional interface used to indicate if arithmetic is supported.
        /// </summary>
        public delegate bool SupportsArithmetic();


        /// <summary>
        /// Indicates whether arithmetic operations are supported.
        /// Default implementation allows arithmetic.
        /// TemperatureUnit will override this.
        /// </summary>
        public bool SupportsArithmeticOperations()
        {
            return true;
        }

        /// <summary>
        /// Validates if the arithmetic operation is supported.
        /// Default implementation allows all operations.
        /// TemperatureUnit overrides this method.
        /// </summary>
        public virtual void ValidateOperationSupport(string operation)
        {
            // Default implementation allows arithmetic
        }
    }
}