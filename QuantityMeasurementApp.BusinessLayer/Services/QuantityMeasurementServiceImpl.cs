using QuantityMeasurementApp.BusinessLayer.Interfaces;
using QuantityMeasurementApp.ModelLayer.DTO;
using QuantityMeasurementApp.ModelLayer.Entities;
using QuantityMeasurementApp.ModelLayer.Models;
using QuantityMeasurementApp.RepoLayer.Interfaces;
using System.Collections.Generic;

namespace QuantityMeasurementApp.BusinessLayer.Services
{
    public class QuantityMeasurementServiceImpl : IQuantityMeasurementService
    {
        private readonly IQuantityMeasurementRepository _repository;

        public QuantityMeasurementServiceImpl(IQuantityMeasurementRepository repository)
        {
            _repository = repository;
        }

        public bool Compare(QuantityDTO firstQuantity, QuantityDTO secondQuantity)
        {
            var first  = new Quantity<object>(firstQuantity.Value,  firstQuantity.Unit);
            var second = new Quantity<object>(secondQuantity.Value, secondQuantity.Unit);
            bool result = first.Equals(second);
            _repository.Save(new QuantityMeasurementEntity(
                "COMPARE", first.ToString(), second.ToString(),
                result.ToString(), GetMeasureType(firstQuantity.Unit)));
            return result;
        }

        public QuantityDTO Convert(QuantityDTO sourceQuantity, object targetUnit)
        {
            var quantity = new Quantity<object>(sourceQuantity.Value, sourceQuantity.Unit);
            var result   = quantity.ConvertTo(targetUnit);
            _repository.Save(new QuantityMeasurementEntity(
                "CONVERT", quantity.ToString(), "-",
                result.ToString(), GetMeasureType(sourceQuantity.Unit)));
            return new QuantityDTO(result.Value, targetUnit);
        }

        public QuantityDTO Add(
            QuantityDTO firstQuantity, QuantityDTO secondQuantity, object targetUnit)
        {
            var first  = new Quantity<object>(firstQuantity.Value,  firstQuantity.Unit);
            var second = new Quantity<object>(secondQuantity.Value, secondQuantity.Unit);
            var result = first.Add(second, targetUnit);
            _repository.Save(new QuantityMeasurementEntity(
                "ADD", first.ToString(), second.ToString(),
                result.ToString(), GetMeasureType(firstQuantity.Unit)));
            return new QuantityDTO(result.Value, targetUnit);
        }

        public QuantityDTO Subtract(QuantityDTO firstQuantity, QuantityDTO secondQuantity)
        {
            var first  = new Quantity<object>(firstQuantity.Value,  firstQuantity.Unit);
            var second = new Quantity<object>(secondQuantity.Value, secondQuantity.Unit);
            var result = first.Subtract(second);
            _repository.Save(new QuantityMeasurementEntity(
                "SUBTRACT", first.ToString(), second.ToString(),
                result.ToString(), GetMeasureType(firstQuantity.Unit)));
            return new QuantityDTO(result.Value, result.Unit);
        }

        public double Divide(QuantityDTO firstQuantity, QuantityDTO secondQuantity)
        {
            var first  = new Quantity<object>(firstQuantity.Value,  firstQuantity.Unit);
            var second = new Quantity<object>(secondQuantity.Value, secondQuantity.Unit);
            double result = first.Divide(second);
            _repository.Save(new QuantityMeasurementEntity(
                "DIVIDE", first.ToString(), second.ToString(),
                result.ToString(), GetMeasureType(firstQuantity.Unit)));
            return result;
        }

        public List<QuantityMeasurementEntity> GetAllMeasurements() =>
            _repository.GetAll();

        public List<QuantityMeasurementEntity> GetByOperation(string operation) =>
            _repository.GetByOperation(operation);

        public List<QuantityMeasurementEntity> GetByMeasureType(string measureType) =>
            _repository.GetByMeasureType(measureType);

        public int GetTotalCount() =>
            _repository.GetTotalCount();

        public void DeleteAll() =>
            _repository.DeleteAll();

        public string GetPoolStats() =>
            _repository.GetPoolStats();

        private string GetMeasureType(object unit)
        {
            if (unit is LengthEnum)      return "LENGTH";
            if (unit is WeightEnum)      return "WEIGHT";
            if (unit is VolumeEnum)      return "VOLUME";
            if (unit is TemperatureEnum) return "TEMPERATURE";
            return "UNKNOWN";
        }
    }
}