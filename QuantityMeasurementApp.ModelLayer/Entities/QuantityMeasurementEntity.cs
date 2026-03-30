using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuantityMeasurementApp.ModelLayer.Entities
{
    /// <summary>EF Core entity mapped to the quantity_measurements table.</summary>
    [Table("quantity_measurements")]
    public class QuantityMeasurementEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required][Column("operation")][MaxLength(50)]
        public string Operation { get; set; }

        /// <summary>Nullable — not present for error records.</summary>
        [Column("operand_one")][MaxLength(200)]
        public string? OperandOne { get; set; }

        [Column("operand_two")][MaxLength(200)]
        public string? OperandTwo { get; set; }

        [Column("result")][MaxLength(200)]
        public string? Result { get; set; }

        [Column("measure_type")][MaxLength(50)]
        public string? MeasureType { get; set; }

        /// <summary>True when this record represents a failed operation.</summary>
        [Column("is_error")]
        public bool IsError { get; set; } = false;

        /// <summary>Nullable — only set when IsError is true.</summary>
        [Column("error_message")][MaxLength(500)]
        public string? ErrorMessage { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public QuantityMeasurementEntity() { }

        public QuantityMeasurementEntity(
            string  operation,
            string? operandOne   = null,
            string? operandTwo   = null,
            string? result       = null,
            string? measureType  = null,
            bool    isError      = false,
            string? errorMessage = null)
        {
            Operation    = operation;
            OperandOne   = operandOne;
            OperandTwo   = operandTwo;
            Result       = result;
            MeasureType  = measureType;
            IsError      = isError;
            ErrorMessage = errorMessage;
        }
    }
}
