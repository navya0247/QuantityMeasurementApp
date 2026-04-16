using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QmaService.DTO;
using QmaService.Enums;
using QmaService.Interfaces;

namespace QmaService.Controllers
{
    /// <summary>
    /// REST endpoints for quantity measurement operations.
    /// Compare / Convert / Add / Subtract / Divide → open to guests (AllowAnonymous)
    /// History / Delete → require JWT (Authorize)
    /// </summary>
    [ApiController]
    [Route("measurements")]
    [Produces("application/json")]
    public class QuantityController : ControllerBase
    {
        private readonly IQuantityMeasurementService _service;

        public QuantityController(IQuantityMeasurementService service)
            => _service = service;

        //  Operations — open to guests 

        /// <summary>Compare two quantities for equality.</summary>
        [HttpPost("compare")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(QuantityMeasurementDTO), 200)]
        [ProducesResponseType(400)]
        public IActionResult Compare([FromBody] QuantityInputDTO input)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var (first, second) = ResolveStandard(input);
            return Ok(_service.Compare(first, second));
        }

        /// <summary>Convert a quantity to a different unit.</summary>
        [HttpPost("convert")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(QuantityMeasurementDTO), 200)]
        [ProducesResponseType(400)]
        public IActionResult Convert([FromBody] QuantityInputDTO input)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var source    = ResolveDTO(input.ThisQuantity);
            object target = ResolveUnit(input.ThatQuantity.Unit, input.ThatQuantity.MeasurementType);
            return Ok(_service.Convert(source, target));
        }

        /// <summary>Add two quantities. TargetUnit is optional.</summary>
        [HttpPost("add")]
        [AllowAnonymous]
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

        /// <summary>Subtract second quantity from first.</summary>
        [HttpPost("subtract")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(QuantityMeasurementDTO), 200)]
        [ProducesResponseType(400)]
        public IActionResult Subtract([FromBody] ArithmeticInputDTO input)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var first  = ResolveDTO(input.ThisQuantity);
            var second = ResolveDTO(input.ThatQuantity);
            return Ok(_service.Subtract(first, second));
        }

        /// <summary>Divide first quantity by second. Returns scalar ratio.</summary>
        [HttpPost("divide")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(QuantityMeasurementDTO), 200)]
        [ProducesResponseType(400)]
        public IActionResult Divide([FromBody] QuantityInputDTO input)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var (first, second) = ResolveStandard(input);
            return Ok(_service.Divide(first, second));
        }

        //  History — require JWT 

        /// <summary>Returns all measurement history.</summary>
        [HttpGet("history")]
        [Authorize]
        [ProducesResponseType(typeof(List<QuantityMeasurementDTO>), 200)]
        public IActionResult GetAllHistory() => Ok(_service.GetAllMeasurements());

        /// <summary>Filter history by operation: COMPARE | CONVERT | ADD | SUBTRACT | DIVIDE</summary>
        [HttpGet("history/operation/{operation}")]
        [Authorize]
        [ProducesResponseType(typeof(List<QuantityMeasurementDTO>), 200)]
        public IActionResult GetByOperation(string operation)
            => Ok(_service.GetByOperation(operation.ToUpper()));

        /// <summary>Filter history by type: LENGTH | WEIGHT | VOLUME | TEMPERATURE</summary>
        [HttpGet("history/type/{measureType}")]
        [Authorize]
        [ProducesResponseType(typeof(List<QuantityMeasurementDTO>), 200)]
        public IActionResult GetByMeasureType(string measureType)
            => Ok(_service.GetByMeasureType(measureType.ToUpper()));

        /// <summary>Returns all records where an error occurred.</summary>
        [HttpGet("history/errored")]
        [Authorize]
        [ProducesResponseType(typeof(List<QuantityMeasurementDTO>), 200)]
        public IActionResult GetErrored() => Ok(_service.GetErrored());

        /// <summary>Returns count of records for a specific operation.</summary>
        [HttpGet("count/{operation}")]
        [Authorize]
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

        // ── Helpers ───────────────────────────────────────────────────

        private (QuantityDTO, QuantityDTO) ResolveStandard(QuantityInputDTO input)
            => (ResolveDTO(input.ThisQuantity), ResolveDTO(input.ThatQuantity));

        private QuantityDTO ResolveDTO(QuantityRequestDTO req)
            => new QuantityDTO(req.Value, ResolveUnit(req.Unit, req.MeasurementType));

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
