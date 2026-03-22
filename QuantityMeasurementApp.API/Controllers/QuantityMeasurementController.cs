using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuantityMeasurementApp.BusinessLayer.Interfaces;
using QuantityMeasurementApp.ModelLayer.DTO;
using QuantityMeasurementApp.ModelLayer.Models;

namespace QuantityMeasurementApp.API.Controllers
{
    /// <summary>
    /// REST endpoints for quantity measurement operations. JWT required for all.
    /// </summary>
    [ApiController]
    [Route("measurements")]
    [Authorize]
    [Produces("application/json")]
    public class QuantityMeasurementController : ControllerBase
    {
        private readonly IQuantityMeasurementService _service;

        public QuantityMeasurementController(IQuantityMeasurementService service)
            => _service = service;

        //  POST /measurements/compare 

        /// <summary>Compare two quantities for equality.</summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///       "thisQuantity": { "value": 1.0,  "unit": "FEET",   "measurementType": "LengthUnit" },
        ///       "thatQuantity": { "value": 12.0, "unit": "INCHES", "measurementType": "LengthUnit" }
        ///     }
        ///
        /// Expected result: true (1 FEET == 12 INCHES)
        /// </remarks>
        [HttpPost("compare")]
        [ProducesResponseType(typeof(QuantityMeasurementDTO), 200)]
        [ProducesResponseType(400)]
        public IActionResult Compare([FromBody] QuantityInputDTO input)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var (first, second) = ResolveStandard(input);
            return Ok(_service.Compare(first, second));
        }

        //  POST /measurements/convert 

        /// <summary>Convert a quantity to a different unit.</summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///       "thisQuantity": { "value": 1.0, "unit": "FEET",   "measurementType": "LengthUnit" },
        ///       "thatQuantity": { "value": 0.0, "unit": "INCHES", "measurementType": "LengthUnit" }
        ///     }
        ///
        /// Expected result: 12 INCHES (1 FEET converted to INCHES)
        /// </remarks>
        [HttpPost("convert")]
        [ProducesResponseType(typeof(QuantityMeasurementDTO), 200)]
        [ProducesResponseType(400)]
        public IActionResult Convert([FromBody] QuantityInputDTO input)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var source    = ResolveDTO(input.ThisQuantity);
            object target = ResolveUnit(input.ThatQuantity.Unit, input.ThatQuantity.MeasurementType);
            return Ok(_service.Convert(source, target));
        }

        // POST /measurements/add

        /// <summary>Add two quantities. TargetUnit is optional.</summary>
        /// <remarks>
        /// Sample without targetUnit (result in first quantity unit):
        ///
        ///     {
        ///       "thisQuantity": { "value": 1.0,  "unit": "FEET",   "measurementType": "LengthUnit" },
        ///       "thatQuantity": { "value": 12.0, "unit": "INCHES", "measurementType": "LengthUnit" }
        ///     }
        ///
        /// Sample with targetUnit (result in YARDS):
        ///
        ///     {
        ///       "thisQuantity": { "value": 1.0,  "unit": "FEET",   "measurementType": "LengthUnit" },
        ///       "thatQuantity": { "value": 12.0, "unit": "INCHES", "measurementType": "LengthUnit" },
        ///       "targetUnit":   { "value": 0.0,  "unit": "YARDS",  "measurementType": "LengthUnit" }
        ///     }
        ///
        /// Expected result without targetUnit: 2 FEET
        /// </remarks>
        [HttpPost("add")]
        [ProducesResponseType(typeof(QuantityMeasurementDTO), 200)]
        [ProducesResponseType(400)]
        public IActionResult Add([FromBody] ArithmeticInputDTO input)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var first     = ResolveDTO(input.ThisQuantity);
            var second    = ResolveDTO(input.ThatQuantity);
            object target = input.TargetUnit != null
                ? ResolveUnit(input.TargetUnit.Unit, input.TargetUnit.MeasurementType)
                : ResolveUnit(input.ThisQuantity.Unit, input.ThisQuantity.MeasurementType);
            return Ok(_service.Add(first, second, target));
        }

        //  POST /measurements/subtract 

        /// <summary>Subtract second quantity from first. TargetUnit is optional.</summary>
        /// <remarks>
        /// Sample without targetUnit:
        ///
        ///     {
        ///       "thisQuantity": { "value": 2.0,   "unit": "KILOGRAM", "measurementType": "WeightUnit" },
        ///       "thatQuantity": { "value": 500.0, "unit": "GRAM",     "measurementType": "WeightUnit" }
        ///     }
        ///
        /// Sample with targetUnit (result in GRAM):
        ///
        ///     {
        ///       "thisQuantity": { "value": 2.0,   "unit": "KILOGRAM", "measurementType": "WeightUnit" },
        ///       "thatQuantity": { "value": 500.0, "unit": "GRAM",     "measurementType": "WeightUnit" },
        ///       "targetUnit":   { "value": 0.0,   "unit": "GRAM",     "measurementType": "WeightUnit" }
        ///     }
        ///
        /// Expected result without targetUnit: 1.5 KILOGRAM
        /// </remarks>
        [HttpPost("subtract")]
        [ProducesResponseType(typeof(QuantityMeasurementDTO), 200)]
        [ProducesResponseType(400)]
        public IActionResult Subtract([FromBody] ArithmeticInputDTO input)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var first  = ResolveDTO(input.ThisQuantity);
            var second = ResolveDTO(input.ThatQuantity);
            return Ok(_service.Subtract(first, second));
        }

        //  POST /measurements/divide 

        /// <summary>Divide first quantity by second. Returns scalar ratio.</summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///       "thisQuantity": { "value": 2.0, "unit": "LITRE", "measurementType": "VolumeUnit" },
        ///       "thatQuantity": { "value": 1.0, "unit": "LITRE", "measurementType": "VolumeUnit" }
        ///     }
        ///
        /// Expected result: 2 (2 LITRE divided by 1 LITRE)
        /// </remarks>
        [HttpPost("divide")]
        [ProducesResponseType(typeof(QuantityMeasurementDTO), 200)]
        [ProducesResponseType(400)]
        public IActionResult Divide([FromBody] QuantityInputDTO input)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var (first, second) = ResolveStandard(input);
            return Ok(_service.Divide(first, second));
        }

        //  GET endpoints 

        /// <summary>Returns all measurement history.</summary>
        [HttpGet("history")]
        [ProducesResponseType(typeof(List<QuantityMeasurementDTO>), 200)]
        public IActionResult GetAllHistory() => Ok(_service.GetAllMeasurements());

        /// <summary>Filter history by operation: COMPARE | CONVERT | ADD | SUBTRACT | DIVIDE</summary>
        [HttpGet("history/operation/{operation}")]
        [ProducesResponseType(typeof(List<QuantityMeasurementDTO>), 200)]
        public IActionResult GetByOperation(string operation)
            => Ok(_service.GetByOperation(operation.ToUpper()));

        /// <summary>Filter history by type: LENGTH | WEIGHT | VOLUME | TEMPERATURE</summary>
        [HttpGet("history/type/{measureType}")]
        [ProducesResponseType(typeof(List<QuantityMeasurementDTO>), 200)]
        public IActionResult GetByMeasureType(string measureType)
            => Ok(_service.GetByMeasureType(measureType.ToUpper()));

        /// <summary>Returns all records where an error occurred.</summary>
        [HttpGet("history/errored")]
        [ProducesResponseType(typeof(List<QuantityMeasurementDTO>), 200)]
        public IActionResult GetErrored() => Ok(_service.GetErrored());

        /// <summary>Returns count of records for a specific operation.</summary>
        [HttpGet("count/{operation}")]
        [ProducesResponseType(200)]
        public IActionResult GetCount(string operation)
        {
            var records = _service.GetByOperation(operation.ToUpper());
            return Ok(new { operation = operation.ToUpper(), count = records.Count });
        }

        /// <summary>Deletes all records. Admin role only.</summary>
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(204)]
        public IActionResult DeleteAll()
        {
            _service.DeleteAll();
            return NoContent();
        }

        //  Helpers 

        private (QuantityDTO, QuantityDTO) ResolveStandard(QuantityInputDTO input)
            => (ResolveDTO(input.ThisQuantity), ResolveDTO(input.ThatQuantity));

        private QuantityDTO ResolveDTO(QuantityRequestDTO req)
            => new QuantityDTO(req.Value, ResolveUnit(req.Unit, req.MeasurementType));

        /// <summary>Converts unit string to enum. Case insensitive — FEET, Feet, feet all work.</summary>
        private object ResolveUnit(string unit, string measurementType)
        {
            string u = unit.ToUpperInvariant();
            return measurementType switch
            {
                "LengthUnit" =>
                    Enum.TryParse<LengthEnum>(u, out var le) ? (object)le
                    : throw new ArgumentException(
                        $"Invalid LengthUnit '{unit}'. Valid: FEET, INCHES, YARDS, CENTIMETERS"),
                "WeightUnit" =>
                    Enum.TryParse<WeightEnum>(u, out var we) ? (object)we
                    : throw new ArgumentException(
                        $"Invalid WeightUnit '{unit}'. Valid: KILOGRAM, GRAM, POUND"),
                "VolumeUnit" =>
                    Enum.TryParse<VolumeEnum>(u, out var ve) ? (object)ve
                    : throw new ArgumentException(
                        $"Invalid VolumeUnit '{unit}'. Valid: LITRE, MILLILITRE, GALLON"),
                "TemperatureUnit" =>
                    Enum.TryParse<TemperatureEnum>(u, out var te) ? (object)te
                    : throw new ArgumentException(
                        $"Invalid TemperatureUnit '{unit}'. Valid: CELSIUS, FAHRENHEIT, KELVIN"),
                _ => throw new ArgumentException($"Unknown MeasurementType '{measurementType}'")
            };
        }
    }
}
