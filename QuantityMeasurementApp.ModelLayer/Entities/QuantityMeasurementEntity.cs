using System;

namespace QuantityMeasurementApp.ModelLayer.Entities
{
    // Holds the data for one operation record.
    // MeasureType is added in UC16 to support
    // filtering by LENGTH, WEIGHT, VOLUME, TEMPERATURE.
    public class QuantityMeasurementEntity
    {
        public Guid   Id          { get; set; }
        public string Operation   { get; set; }
        public string OperandOne  { get; set; }
        public string OperandTwo  { get; set; }
        public string Result      { get; set; }
        public string MeasureType { get; set; }

        public QuantityMeasurementEntity(
            string operation,
            string operandOne,
            string operandTwo,
            string result,
            string measureType = "")
        {
            Id          = Guid.NewGuid();
            Operation   = operation;
            OperandOne  = operandOne;
            OperandTwo  = operandTwo;
            Result      = result;
            MeasureType = measureType;
        }
    }
}