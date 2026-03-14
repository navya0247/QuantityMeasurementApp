using QuantityMeasurementApp.ModelLayer.DTO;
using QuantityMeasurementApp.ModelLayer.Models;
using QuantityMeasurementApp.ModelLayer.Entities;
using QuantityMeasurementApp.RepoLayer.Interfaces;
using QuantityMeasurementApp.BusinessLayer.Interfaces;

namespace QuantityMeasurementApp.BusinessLayer.Services
{
    /// <summary>
    /// Service implementation containing business logic
    /// </summary>
    public class QuantityMeasurementServiceImpl : IQuantityMeasurementService
    {
        private readonly IQuantityMeasurementRepository repository;

        public QuantityMeasurementServiceImpl(IQuantityMeasurementRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// UC1–UC4, UC9, UC11, UC14
        /// Generic equality comparison.
        /// </summary>
        public bool Compare(QuantityDTO firstQuantity, QuantityDTO secondQuantity)
        {
            var first = new Quantity<object>(firstQuantity.Value, firstQuantity.Unit);
            var second = new Quantity<object>(secondQuantity.Value, secondQuantity.Unit);

            bool result = first.Equals(second);

            repository.Save(new QuantityMeasurementEntity(
                "COMPARE",
                first.ToString(),
                second.ToString(),
                result.ToString()));

            return result;
        }

        /// <summary>
        /// UC5, UC8, UC10
        /// Converts quantity to target unit.
        /// </summary>
        public QuantityDTO Convert(QuantityDTO sourceQuantity, object targetUnit)
        {
            var quantity = new Quantity<object>(sourceQuantity.Value, sourceQuantity.Unit);

            var result = quantity.ConvertTo(targetUnit);

            repository.Save(new QuantityMeasurementEntity(
                "CONVERT",
                quantity.ToString(),
                "-",
                result.ToString()));

            return new QuantityDTO(result.Value, targetUnit);
        }

        /// <summary>
        /// UC6, UC7, UC10
        /// Adds two quantities.
        /// </summary>
        public QuantityDTO Add(QuantityDTO firstQuantity, QuantityDTO secondQuantity, object targetUnit)
        {
            var first = new Quantity<object>(firstQuantity.Value, firstQuantity.Unit);
            var second = new Quantity<object>(secondQuantity.Value, secondQuantity.Unit);

            var result = first.Add(second, targetUnit);

            repository.Save(new QuantityMeasurementEntity(
                "ADD",
                first.ToString(),
                second.ToString(),
                result.ToString()));

            return new QuantityDTO(result.Value, targetUnit);
        }

        /// <summary>
        /// UC12
        /// Subtracts one quantity from another.
        /// </summary>
        public QuantityDTO Subtract(QuantityDTO firstQuantity, QuantityDTO secondQuantity)
        {
            var first = new Quantity<object>(firstQuantity.Value, firstQuantity.Unit);
            var second = new Quantity<object>(secondQuantity.Value, secondQuantity.Unit);

            var result = first.Subtract(second);

            repository.Save(new QuantityMeasurementEntity(
                "SUBTRACT",
                first.ToString(),
                second.ToString(),
                result.ToString()));

            return new QuantityDTO(result.Value, result.Unit);
        }

        /// <summary>
        /// UC13
        /// Divides two quantities.
        /// </summary>
        public double Divide(QuantityDTO firstQuantity, QuantityDTO secondQuantity)
        {
            var first = new Quantity<object>(firstQuantity.Value, firstQuantity.Unit);
            var second = new Quantity<object>(secondQuantity.Value, secondQuantity.Unit);

            double result = first.Divide(second);

            repository.Save(new QuantityMeasurementEntity(
                "DIVIDE",
                first.ToString(),
                second.ToString(),
                result.ToString()));

            return result;
        }
    }
}