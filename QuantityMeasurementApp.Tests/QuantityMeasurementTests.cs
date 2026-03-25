using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.ModelLayer.Enums;
using QuantityMeasurementApp.ModelLayer.DTO;
using QuantityMeasurementApp.BusinessLayer.Services;
using QuantityMeasurementApp.RepoLayer.Repositories;

namespace QuantityMeasurementApp.Tests
{
    /// <summary>
    /// UC10 Tests — updated to use QuantityMeasurementServiceImpl directly.
    /// The old static QuantityMeasurementService facade was removed in UC17.
    /// </summary>
    [TestClass]
    public class QuantityMeasurementTests
    {
        private static QuantityMeasurementServiceImpl _service;

        [ClassInitialize]
        public static void ClassSetup(TestContext _)
        {
            var repo = new InMemoryQuantityRepository();
            var logger = Microsoft.Extensions.Logging.Abstractions.NullLogger<QuantityMeasurementApp.BusinessLayer.Services.QuantityMeasurementServiceImpl>.Instance;
            _service = new QuantityMeasurementServiceImpl(repo, logger);
        }

        // ── UC1 ───────────────────────────────────────────────────────────

        [TestMethod]
        public void FeetEquality_SameValue_ReturnsTrue()
        {
            var dto = _service.Compare(
                new QuantityDTO(1.0, LengthEnum.FEET),
                new QuantityDTO(1.0, LengthEnum.FEET));
            Assert.AreEqual("true", dto.Result);
        }

        [TestMethod]
        public void FeetEquality_DifferentValue_ReturnsFalse()
        {
            var dto = _service.Compare(
                new QuantityDTO(1.0, LengthEnum.FEET),
                new QuantityDTO(2.0, LengthEnum.FEET));
            Assert.AreEqual("false", dto.Result);
        }

        // ── UC2 ───────────────────────────────────────────────────────────

        [TestMethod]
        public void InchEquality_SameValue_ReturnsTrue()
        {
            var dto = _service.Compare(
                new QuantityDTO(5.0, LengthEnum.INCHES),
                new QuantityDTO(5.0, LengthEnum.INCHES));
            Assert.AreEqual("true", dto.Result);
        }

        [TestMethod]
        public void InchEquality_DifferentValue_ReturnsFalse()
        {
            var dto = _service.Compare(
                new QuantityDTO(5.0, LengthEnum.INCHES),
                new QuantityDTO(6.0, LengthEnum.INCHES));
            Assert.AreEqual("false", dto.Result);
        }

        // ── UC3 ───────────────────────────────────────────────────────────

        [TestMethod]
        public void FeetToFeet_SameValue_ReturnsTrue()
        {
            var dto = _service.Compare(
                new QuantityDTO(1.0, LengthEnum.FEET),
                new QuantityDTO(1.0, LengthEnum.FEET));
            Assert.AreEqual("true", dto.Result);
        }

        [TestMethod]
        public void FeetToInches_Equivalent_ReturnsTrue()
        {
            var dto = _service.Compare(
                new QuantityDTO(1.0, LengthEnum.FEET),
                new QuantityDTO(12.0, LengthEnum.INCHES));
            Assert.AreEqual("true", dto.Result);
        }

        // ── UC4 ───────────────────────────────────────────────────────────

        [TestMethod]
        public void YardToFeet_Equivalent_ReturnsTrue()
        {
            var dto = _service.Compare(
                new QuantityDTO(1.0, LengthEnum.YARDS),
                new QuantityDTO(3.0, LengthEnum.FEET));
            Assert.AreEqual("true", dto.Result);
        }

        [TestMethod]
        public void CmToInch_Equivalent_ReturnsTrue()
        {
            var dto = _service.Compare(
                new QuantityDTO(2.54, LengthEnum.CENTIMETERS),
                new QuantityDTO(1.0, LengthEnum.INCHES));
            Assert.AreEqual("true", dto.Result);
        }

        // ── UC5 ───────────────────────────────────────────────────────────

        [TestMethod]
        public void Convert_FeetToInches_Returns12()
        {
            var dto = _service.Convert(
                new QuantityDTO(1.0, LengthEnum.FEET), LengthEnum.INCHES);
            Assert.IsTrue(dto.Result.Contains("12"));
        }

        [TestMethod]
        public void Convert_InchesToFeet_Returns2()
        {
            var dto = _service.Convert(
                new QuantityDTO(24.0, LengthEnum.INCHES), LengthEnum.FEET);
            Assert.IsTrue(dto.Result.Contains("2"));
        }

        // ── UC6 ───────────────────────────────────────────────────────────

        [TestMethod]
        public void Add_FeetPlusFeet_Returns3Feet()
        {
            var dto = _service.Add(
                new QuantityDTO(1.0, LengthEnum.FEET),
                new QuantityDTO(2.0, LengthEnum.FEET),
                LengthEnum.FEET);
            Assert.IsTrue(dto.Result.Contains("3"));
        }

        [TestMethod]
        public void Add_FeetPlusInches_Returns2Feet()
        {
            var dto = _service.Add(
                new QuantityDTO(1.0, LengthEnum.FEET),
                new QuantityDTO(12.0, LengthEnum.INCHES),
                LengthEnum.FEET);
            Assert.IsTrue(dto.Result.Contains("2"));
        }

        // ── UC8 ───────────────────────────────────────────────────────────

        [TestMethod]
        public void LengthUnit_ConvertToBaseUnit()
        {
            double result = LengthUnit.ConvertToBaseUnit(LengthEnum.INCHES, 12);
            Assert.AreEqual(1.0, result, 0.0001);
        }

        [TestMethod]
        public void LengthUnit_ConvertFromBaseUnit()
        {
            double result = LengthUnit.ConvertFromBaseUnit(LengthEnum.INCHES, 1);
            Assert.AreEqual(12.0, result, 0.0001);
        }

        // ── UC10 ──────────────────────────────────────────────────────────

        [TestMethod]
        public void GenericQuantity_LengthEquality()
        {
            var q1 = new Quantity<LengthEnum>(1.0, LengthEnum.FEET);
            var q2 = new Quantity<LengthEnum>(12.0, LengthEnum.INCHES);
            Assert.IsTrue(q1.Equals(q2));
        }

        [TestMethod]
        public void GenericQuantity_LengthConversion()
        {
            var q = new Quantity<LengthEnum>(1.0, LengthEnum.FEET);
            var result = q.ConvertTo(LengthEnum.INCHES);
            Assert.AreEqual(12.0, result.Value, 0.0001);
        }

        [TestMethod]
        public void GenericQuantity_LengthAddition()
        {
            var q1 = new Quantity<LengthEnum>(1.0, LengthEnum.FEET);
            var q2 = new Quantity<LengthEnum>(12.0, LengthEnum.INCHES);
            var result = q1.Add(q2, LengthEnum.FEET);
            Assert.AreEqual(2.0, result.Value, 0.0001);
        }
    }
}
