using System;
using System.Collections.Generic;
using System.Linq;
using QuantityMeasurementApp.ModelLayer.Entities;

namespace QuantityMeasurementApp.ModelLayer.DTO
{
    /// <summary>
    /// DTO for quantity measurement operations exchanged between API and service layers.
    /// </summary>
    public class QuantityMeasurementDTO
    {
        public string Id           { get; set; }
        public string Operation    { get; set; }
        public string OperandOne   { get; set; }
        public string OperandTwo   { get; set; }
        public string Result       { get; set; }
        public string MeasureType  { get; set; }
        public bool   IsError      { get; set; }
        public string ErrorMessage { get; set; }
        public string CreatedAt    { get; set; }

        //  Entity - DTO 

        /// <summary>Converts a single entity to its DTO representation.</summary>
        public static QuantityMeasurementDTO FromEntity(QuantityMeasurementEntity entity)
        {
            if (entity == null) return null;
            return new QuantityMeasurementDTO
            {
                Id           = entity.Id.ToString(),
                Operation    = entity.Operation,
                OperandOne   = entity.OperandOne,
                OperandTwo   = entity.OperandTwo,
                Result       = entity.Result,
                MeasureType  = entity.MeasureType,
                IsError      = entity.IsError,
                ErrorMessage = entity.ErrorMessage,
                CreatedAt    = entity.CreatedAt.ToString("o")
            };
        }

        /// <summary>Converts a list of entities to DTOs using LINQ.</summary>
        public static List<QuantityMeasurementDTO> FromEntityList(
            IEnumerable<QuantityMeasurementEntity> entities)
            => entities?.Select(FromEntity).ToList() ?? new List<QuantityMeasurementDTO>();

        //  DTO - Entity 

        /// <summary>Converts this DTO back to a persistence entity.</summary>
        public QuantityMeasurementEntity ToEntity() =>
            new QuantityMeasurementEntity(
                Operation, OperandOne, OperandTwo,
                Result, MeasureType, IsError, ErrorMessage)
            {
                Id        = string.IsNullOrEmpty(Id) ? Guid.NewGuid() : Guid.Parse(Id),
                CreatedAt = string.IsNullOrEmpty(CreatedAt) ? DateTime.UtcNow : DateTime.Parse(CreatedAt)
            };

        /// <summary>Converts a list of DTOs to entities using LINQ.</summary>
        public static List<QuantityMeasurementEntity> ToEntityList(
            IEnumerable<QuantityMeasurementDTO> dtos)
            => dtos?.Select(d => d.ToEntity()).ToList() ?? new List<QuantityMeasurementEntity>();

        /// <summary>Creates an error DTO for failed operations.</summary>
        public static QuantityMeasurementDTO Error(
            string operation, string measureType, string message)
            => new QuantityMeasurementDTO
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
