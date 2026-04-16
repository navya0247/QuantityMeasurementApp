using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using QmaService.Entities;

namespace QmaService.DTO
{
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

    public class QuantityRequestDTO
    {
        [Required] public double Value           { get; set; } = 1.0;
        [Required] public string Unit            { get; set; } = "FEET";
        [Required] public string MeasurementType { get; set; } = "LengthUnit";
    }

    public class QuantityInputDTO
    {
        [Required] public QuantityRequestDTO ThisQuantity { get; set; } = new();
        [Required] public QuantityRequestDTO ThatQuantity { get; set; } = new();
    }

    public class ArithmeticInputDTO
    {
        [Required] public QuantityRequestDTO  ThisQuantity { get; set; } = new();
        [Required] public QuantityRequestDTO  ThatQuantity { get; set; } = new();
        public            QuantityRequestDTO? TargetUnit   { get; set; }
    }

    public class QuantityMeasurementDTO
    {
        public string  Id           { get; set; } = string.Empty;
        public string  Operation    { get; set; } = string.Empty;
        public string? OperandOne   { get; set; }
        public string? OperandTwo   { get; set; }
        public string? Result       { get; set; }
        public string? MeasureType  { get; set; }
        public bool    IsError      { get; set; }
        public string? ErrorMessage { get; set; }
        public string  CreatedAt    { get; set; } = string.Empty;

        public static QuantityMeasurementDTO FromEntity(QuantityMeasurementEntity e) => new()
        {
            Id           = e.Id.ToString(),
            Operation    = e.Operation,
            OperandOne   = e.OperandOne,
            OperandTwo   = e.OperandTwo,
            Result       = e.Result,
            MeasureType  = e.MeasureType,
            IsError      = e.IsError,
            ErrorMessage = e.ErrorMessage,
            CreatedAt    = e.CreatedAt.ToString("o")
        };

        public static List<QuantityMeasurementDTO> FromEntityList(
            IEnumerable<QuantityMeasurementEntity> entities)
            => entities?.Select(FromEntity).ToList() ?? new List<QuantityMeasurementDTO>();

        public static QuantityMeasurementDTO Error(
            string operation, string measureType, string message) => new()
        {
            Id           = Guid.NewGuid().ToString(),
            Operation    = operation,
            MeasureType  = measureType,
            IsError      = true,
            ErrorMessage = message,
            CreatedAt    = DateTime.UtcNow.ToString("o")
        };
    }
}
