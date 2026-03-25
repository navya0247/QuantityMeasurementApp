using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.BusinessLayer.Services;
using QuantityMeasurementApp.ModelLayer.DTO;
using QuantityMeasurementApp.ModelLayer.Entities;
using QuantityMeasurementApp.ModelLayer.Enums;
using QuantityMeasurementApp.RepoLayer.Repositories;

namespace QuantityMeasurementApp.Tests
{
    /// <summary>
    /// UC16 - Repository Tests.
    /// ADO.NET removed — now uses InMemoryQuantityRepository.
    /// All test logic and assertions remain identical.
    /// </summary>
    [TestClass]
    [DoNotParallelize]
    public class UC16_DatabaseRepositoryTests
    {
        private InMemoryQuantityRepository _repository;
        private QuantityMeasurementServiceImpl _service;

        [TestInitialize]
        public void SetUp()
        {
            _repository = new InMemoryQuantityRepository();
            var logger = Microsoft.Extensions.Logging.Abstractions.NullLogger<QuantityMeasurementApp.BusinessLayer.Services.QuantityMeasurementServiceImpl>.Instance;
            _service = new QuantityMeasurementServiceImpl(_repository, logger);
        }

        [TestCleanup]
        public void TearDown() => _repository?.DeleteAll();

        // ── Save Tests ────────────────────────────────────────────────────

        [TestMethod]
        public void Save_SingleEntity_ShouldIncreaseCountToOne()
        {
            _repository.Save(new QuantityMeasurementEntity("COMPARE", "1 FEET", "12 INCHES", "True", "LENGTH"));
            Assert.AreEqual(1, _repository.GetTotalCount());
        }

        [TestMethod]
        public void Save_ThreeEntities_ShouldIncreaseCountToThree()
        {
            _repository.Save(new QuantityMeasurementEntity("COMPARE", "1 FEET", "12 INCHES", "True", "LENGTH"));
            _repository.Save(new QuantityMeasurementEntity("ADD", "1 KILOGRAM", "1000 GRAM", "2 KILOGRAM", "WEIGHT"));
            _repository.Save(new QuantityMeasurementEntity("CONVERT", "1 GALLON", "-", "3.78 LITRE", "VOLUME"));
            Assert.AreEqual(3, _repository.GetTotalCount());
        }

        [TestMethod]
        public void Save_Entity_ShouldPersistAllFieldsCorrectly()
        {
            _repository.Save(new QuantityMeasurementEntity("ADD", "2 FEET", "24 INCHES", "4 FEET", "LENGTH"));
            var result = _repository.GetAll();
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("ADD", result[0].Operation);
            Assert.AreEqual("2 FEET", result[0].OperandOne);
            Assert.AreEqual("24 INCHES", result[0].OperandTwo);
            Assert.AreEqual("4 FEET", result[0].Result);
            Assert.AreEqual("LENGTH", result[0].MeasureType);
        }

        // ── GetAll Tests ──────────────────────────────────────────────────

        [TestMethod]
        public void GetAll_WhenEmpty_ShouldReturnEmptyList()
            => Assert.AreEqual(0, _repository.GetAll().Count);

        [TestMethod]
        public void GetAll_AfterSavingTwo_ShouldReturnTwo()
        {
            _repository.Save(new QuantityMeasurementEntity("COMPARE", "1 FEET", "12 INCHES", "True", "LENGTH"));
            _repository.Save(new QuantityMeasurementEntity("ADD", "1 KG", "1000 GRAM", "2 KG", "WEIGHT"));
            Assert.AreEqual(2, _repository.GetAll().Count);
        }

        // ── GetByOperation Tests ──────────────────────────────────────────

        [TestMethod]
        public void GetByOperation_Compare_ShouldReturnOnlyCompareRecords()
        {
            _repository.Save(new QuantityMeasurementEntity("COMPARE", "1 FEET", "12 INCHES", "True", "LENGTH"));
            _repository.Save(new QuantityMeasurementEntity("ADD", "1 FEET", "1 FEET", "2 FEET", "LENGTH"));
            _repository.Save(new QuantityMeasurementEntity("COMPARE", "1 KG", "1000 GRAM", "True", "WEIGHT"));
            var result = _repository.GetByOperation("COMPARE");
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.TrueForAll(e => e.Operation == "COMPARE"));
        }

        [TestMethod]
        public void GetByOperation_NoMatch_ShouldReturnEmptyList()
        {
            _repository.Save(new QuantityMeasurementEntity("COMPARE", "1 FEET", "12 INCHES", "True", "LENGTH"));
            Assert.AreEqual(0, _repository.GetByOperation("DIVIDE").Count);
        }

        // ── GetByMeasureType Tests ────────────────────────────────────────

        [TestMethod]
        public void GetByMeasureType_Length_ShouldReturnOnlyLengthRecords()
        {
            _repository.Save(new QuantityMeasurementEntity("COMPARE", "1 FEET", "12 INCHES", "True", "LENGTH"));
            _repository.Save(new QuantityMeasurementEntity("ADD", "1 KG", "1000 GRAM", "2 KG", "WEIGHT"));
            _repository.Save(new QuantityMeasurementEntity("CONVERT", "1 LITRE", "-", "1000 ML", "VOLUME"));
            var result = _repository.GetByMeasureType("LENGTH");
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("LENGTH", result[0].MeasureType);
        }

        // ── GetTotalCount Tests ───────────────────────────────────────────

        [TestMethod]
        public void GetTotalCount_EmptyRepository_ShouldReturnZero()
            => Assert.AreEqual(0, _repository.GetTotalCount());

        [TestMethod]
        public void GetTotalCount_AfterSavingFive_ShouldReturnFive()
        {
            for (int i = 0; i < 5; i++)
                _repository.Save(new QuantityMeasurementEntity("COMPARE", "1 FEET", "12 INCHES", "True", "LENGTH"));
            Assert.AreEqual(5, _repository.GetTotalCount());
        }

        // ── DeleteAll Tests ───────────────────────────────────────────────

        [TestMethod]
        public void DeleteAll_ShouldClearAllRecords()
        {
            _repository.Save(new QuantityMeasurementEntity("COMPARE", "1 FEET", "12 INCHES", "True", "LENGTH"));
            _repository.DeleteAll();
            Assert.AreEqual(0, _repository.GetTotalCount());
        }

        // ── Integration Tests ─────────────────────────────────────────────

        [TestMethod]
        public void Integration_CompareLength_ShouldSaveToRepository()
        {
            var dto = _service.Compare(
                new QuantityDTO(1, LengthEnum.FEET), new QuantityDTO(12, LengthEnum.INCHES));
            Assert.AreEqual("true", dto.Result);
            Assert.AreEqual(1, _repository.GetTotalCount());
        }

        [TestMethod]
        public void Integration_AddVolume_ShouldSaveToRepository()
        {
            var dto = _service.Add(
                new QuantityDTO(1, VolumeEnum.LITRE),
                new QuantityDTO(1000, VolumeEnum.MILLILITRE),
                VolumeEnum.LITRE);
            Assert.IsTrue(dto.Result.Contains("2"));
            Assert.AreEqual(1, _repository.GetTotalCount());
        }

        [TestMethod]
        public void Integration_ConvertLength_ShouldSaveToRepository()
        {
            var dto = _service.Convert(new QuantityDTO(1, LengthEnum.FEET), LengthEnum.INCHES);
            Assert.IsTrue(dto.Result.Contains("12"));
            Assert.AreEqual(1, _repository.GetTotalCount());
        }

        [TestMethod]
        public void Integration_MultipleOperations_ShouldSaveAll()
        {
            _service.Compare(new QuantityDTO(1, LengthEnum.FEET), new QuantityDTO(12, LengthEnum.INCHES));
            _service.Add(new QuantityDTO(1, WeightEnum.KILOGRAM), new QuantityDTO(1000, WeightEnum.GRAM), WeightEnum.KILOGRAM);
            _service.Convert(new QuantityDTO(1, VolumeEnum.LITRE), VolumeEnum.MILLILITRE);
            Assert.AreEqual(3, _repository.GetTotalCount());
        }

        [TestMethod]
        public void Integration_DeleteAll_ShouldClearAll()
        {
            _service.Compare(new QuantityDTO(1, LengthEnum.FEET), new QuantityDTO(12, LengthEnum.INCHES));
            _service.DeleteAll();
            Assert.AreEqual(0, _repository.GetTotalCount());
        }
    }
}
