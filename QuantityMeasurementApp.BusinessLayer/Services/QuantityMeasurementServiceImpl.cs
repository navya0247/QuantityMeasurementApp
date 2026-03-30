using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using QuantityMeasurementApp.BusinessLayer.Interfaces;
using QuantityMeasurementApp.ModelLayer.DTO;
using QuantityMeasurementApp.ModelLayer.Entities;
using QuantityMeasurementApp.ModelLayer.Enums;
using QuantityMeasurementApp.RepoLayer.Interfaces;
using SystemException = System.Exception;

namespace QuantityMeasurementApp.BusinessLayer.Services
{
    /// <summary>
    /// Implements all quantity measurement operations.
    /// Uses ILogger for structured logging of all operations and errors.
    /// </summary>
    public class QuantityMeasurementServiceImpl : IQuantityMeasurementService
    {
        private readonly IQuantityMeasurementRepository          _repository;
        private readonly ILogger<QuantityMeasurementServiceImpl> _logger;

        public QuantityMeasurementServiceImpl(
            IQuantityMeasurementRepository          repository,
            ILogger<QuantityMeasurementServiceImpl> logger)
        {
            _repository = repository;
            _logger     = logger;
        }

        public QuantityMeasurementDTO Compare(QuantityDTO first, QuantityDTO second)
        {
            _logger.LogInformation("Compare started.");
            try
            {
                var q1      = new Quantity<object>(first.Value,  first.Unit);
                var q2      = new Quantity<object>(second.Value, second.Unit);
                bool result = q1.Equals(q2);
                _logger.LogInformation("Compare result: {Result}", result);
                return SaveAndReturn(OperationType.COMPARE, q1.ToString(), q2.ToString(),
                    result.ToString().ToLower(), GetMeasureType(first.Unit));
            }
            catch (SystemException ex)
            {
                _logger.LogError(ex, "Compare failed.");
                return SaveError(OperationType.COMPARE, first.Unit, ex.Message);
            }
        }

        public QuantityMeasurementDTO Convert(QuantityDTO source, object targetUnit)
        {
            _logger.LogInformation("Convert started.");
            try
            {
                var quantity = new Quantity<object>(source.Value, source.Unit);
                var result   = quantity.ConvertTo(targetUnit);
                _logger.LogInformation("Convert result: {Result}", result);
                return SaveAndReturn(OperationType.CONVERT, quantity.ToString(),
                    $"→ {targetUnit}", result.ToString(), GetMeasureType(source.Unit));
            }
            catch (SystemException ex)
            {
                _logger.LogError(ex, "Convert failed.");
                return SaveError(OperationType.CONVERT, source.Unit, ex.Message);
            }
        }

        public QuantityMeasurementDTO Add(QuantityDTO first, QuantityDTO second, object targetUnit)
        {
            _logger.LogInformation("Add started.");
            try
            {
                var q1     = new Quantity<object>(first.Value,  first.Unit);
                var q2     = new Quantity<object>(second.Value, second.Unit);
                var result = q1.Add(q2, targetUnit);
                _logger.LogInformation("Add result: {Result}", result);
                return SaveAndReturn(OperationType.ADD, q1.ToString(), q2.ToString(),
                    result.ToString(), GetMeasureType(first.Unit));
            }
            catch (SystemException ex)
            {
                _logger.LogError(ex, "Add failed.");
                return SaveError(OperationType.ADD, first.Unit, ex.Message);
            }
        }

        public QuantityMeasurementDTO Subtract(QuantityDTO first, QuantityDTO second)
        {
            _logger.LogInformation("Subtract started.");
            try
            {
                var q1     = new Quantity<object>(first.Value,  first.Unit);
                var q2     = new Quantity<object>(second.Value, second.Unit);
                var result = q1.Subtract(q2);
                _logger.LogInformation("Subtract result: {Result}", result);
                return SaveAndReturn(OperationType.SUBTRACT, q1.ToString(), q2.ToString(),
                    result.ToString(), GetMeasureType(first.Unit));
            }
            catch (SystemException ex)
            {
                _logger.LogError(ex, "Subtract failed.");
                return SaveError(OperationType.SUBTRACT, first.Unit, ex.Message);
            }
        }

        public QuantityMeasurementDTO Divide(QuantityDTO first, QuantityDTO second)
        {
            _logger.LogInformation("Divide started.");
            try
            {
                var q1        = new Quantity<object>(first.Value,  first.Unit);
                var q2        = new Quantity<object>(second.Value, second.Unit);
                double result = q1.Divide(q2);
                _logger.LogInformation("Divide result: {Result}", result);
                return SaveAndReturn(OperationType.DIVIDE, q1.ToString(), q2.ToString(),
                    result.ToString("G"), GetMeasureType(first.Unit));
            }
            catch (SystemException ex)
            {
                _logger.LogError(ex, "Divide failed.");
                return SaveError(OperationType.DIVIDE, first.Unit, ex.Message);
            }
        }

        public List<QuantityMeasurementDTO> GetAllMeasurements()
            => QuantityMeasurementDTO.FromEntityList(_repository.GetAll());

        public List<QuantityMeasurementDTO> GetByOperation(string operation)
            => QuantityMeasurementDTO.FromEntityList(_repository.GetByOperation(operation));

        public List<QuantityMeasurementDTO> GetByMeasureType(string measureType)
            => QuantityMeasurementDTO.FromEntityList(_repository.GetByMeasureType(measureType));

        public List<QuantityMeasurementDTO> GetErrored()
            => QuantityMeasurementDTO.FromEntityList(_repository.GetErrored());

        public int  GetTotalCount() => _repository.GetTotalCount();
        public void DeleteAll()     => _repository.DeleteAll();

        private QuantityMeasurementDTO SaveAndReturn(
            OperationType op, string o1, string o2, string result, string measureType)
        {
            var entity = new QuantityMeasurementEntity(
                op.ToString(), o1, o2, result, measureType);
            _repository.Save(entity);
            return QuantityMeasurementDTO.FromEntity(entity);
        }

        private QuantityMeasurementDTO SaveError(
            OperationType op, object unit, string message)
        {
            _logger.LogWarning("Error in {Op}: {Msg}", op, message);
            var entity = new QuantityMeasurementEntity(
                op.ToString(), null, null, null,
                GetMeasureType(unit), isError: true, errorMessage: message);
            _repository.Save(entity);
            return QuantityMeasurementDTO.Error(op.ToString(), GetMeasureType(unit), message);
        }

        private static string GetMeasureType(object unit) => unit switch
        {
            LengthEnum      => "LENGTH",
            WeightEnum      => "WEIGHT",
            VolumeEnum      => "VOLUME",
            TemperatureEnum => "TEMPERATURE",
            _               => "UNKNOWN"
        };
    }
}
