using System;

namespace QuantityMeasurementApp.ModelLayer.Entities
{
    /// <summary>
    /// Entity used to store operation history
    /// inside repository layer.
    /// </summary>
    public class QuantityMeasurementEntity
    {
        public Guid Id { get; set; }

        public string Operation { get; set; }

        public string OperandOne { get; set; }

        public string OperandTwo { get; set; }

        public string Result { get; set; }

        public QuantityMeasurementEntity(string operation, string operandOne,string operandTwo, string result)
        {
            Id = Guid.NewGuid();
            Operation = operation;
            OperandOne = operandOne;
            OperandTwo = operandTwo;
            Result = result;
        }
    }
}