using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.ModelLayer.Entities;
using QuantityMeasurementApp.ModelLayer.Models;
using QuantityMeasurementApp.BusinessLayer.Services;
using QuantityMeasurementApp.ModelLayer.DTO;
using QuantityMeasurementApp.RepoLayer.Repositories;

namespace QuantityMeasurementApp.Tests
{
    /// <summary>
    /// UC16 - Database Integration Tests using ADO.NET with SQL Server.
    /// Tests run sequentially (DoNotParallelize) to avoid shared database conflicts.
    /// Tests cover:
    /// - Save entity to database
    /// - Retrieve all measurements
    /// - Filter by operation type
    /// - Filter by measurement type
    /// - Get total count
    /// - Delete all records
    /// - Pool stats
    /// - SQL injection prevention
    /// - End to end service with database repository
    /// - Cache repository still works independently
    /// </summary>
    [TestClass]
    [DoNotParallelize]
    public class UC16_DatabaseRepositoryTests
    {
        private QuantityMeasurementDatabaseRepository _repository;
        private QuantityMeasurementServiceImpl _service;

        /// <summary>
        /// Runs before every test.
        /// Creates fresh repository using test database and clears all records.
        /// </summary>
        [TestInitialize]
        public void SetUp()
        {
            _repository = new QuantityMeasurementDatabaseRepository(useTestDb: true);
            _service    = new QuantityMeasurementServiceImpl(_repository);
            _repository.DeleteAll();
        }

        /// <summary>
        /// Runs after every test.
        /// Cleans up all test data from database.
        /// </summary>
        [TestCleanup]
        public void TearDown()
        {
            _repository?.DeleteAll();
        }

        // ─────────────────────────────────────────────────────────────────
        // Save Tests
        // ─────────────────────────────────────────────────────────────────

        /// <summary>
        /// Saving one entity should result in count of 1.
        /// </summary>
        [TestMethod]
        public void Save_SingleEntity_ShouldIncreaseCountToOne()
        {
            var entity = new QuantityMeasurementEntity(
                "COMPARE", "1 FEET", "12 INCHES", "True", "LENGTH");

            _repository.Save(entity);

            Assert.AreEqual(1, _repository.GetTotalCount());
        }

        /// <summary>
        /// Saving three entities should result in count of 3.
        /// </summary>
        [TestMethod]
        public void Save_ThreeEntities_ShouldIncreaseCountToThree()
        {
            _repository.Save(new QuantityMeasurementEntity(
                "COMPARE", "1 FEET", "12 INCHES", "True", "LENGTH"));
            _repository.Save(new QuantityMeasurementEntity(
                "ADD", "1 KILOGRAM", "1000 GRAM", "2 KILOGRAM", "WEIGHT"));
            _repository.Save(new QuantityMeasurementEntity(
                "CONVERT", "1 GALLON", "-", "3.78541 LITRE", "VOLUME"));

            Assert.AreEqual(3, _repository.GetTotalCount());
        }

        /// <summary>
        /// All fields saved should be retrievable with correct values.
        /// </summary>
        [TestMethod]
        public void Save_Entity_ShouldPersistAllFieldsCorrectly()
        {
            _repository.Save(new QuantityMeasurementEntity(
                "ADD", "2 FEET", "24 INCHES", "4 FEET", "LENGTH"));

            var result = _repository.GetAll();

            Assert.AreEqual(1,          result.Count);
            Assert.AreEqual("ADD",       result[0].Operation);
            Assert.AreEqual("2 FEET",    result[0].OperandOne);
            Assert.AreEqual("24 INCHES", result[0].OperandTwo);
            Assert.AreEqual("4 FEET",    result[0].Result);
            Assert.AreEqual("LENGTH",    result[0].MeasureType);
        }

        // ─────────────────────────────────────────────────────────────────
        // GetAll Tests
        // ─────────────────────────────────────────────────────────────────

        /// <summary>
        /// GetAll on empty database should return empty list.
        /// </summary>
        [TestMethod]
        public void GetAll_WhenDatabaseEmpty_ShouldReturnEmptyList()
        {
            var result = _repository.GetAll();

            Assert.AreEqual(0, result.Count);
        }

        /// <summary>
        /// GetAll after saving two records should return two records.
        /// </summary>
        [TestMethod]
        public void GetAll_AfterSavingTwo_ShouldReturnTwoRecords()
        {
            _repository.Save(new QuantityMeasurementEntity(
                "COMPARE", "1 FEET", "12 INCHES", "True", "LENGTH"));
            _repository.Save(new QuantityMeasurementEntity(
                "ADD", "1 KG", "1000 GRAM", "2 KG", "WEIGHT"));

            var result = _repository.GetAll();

            Assert.AreEqual(2, result.Count);
        }

        // ─────────────────────────────────────────────────────────────────
        // GetByOperation Tests
        // ─────────────────────────────────────────────────────────────────

        /// <summary>
        /// Filter by COMPARE operation should return only COMPARE records.
        /// </summary>
        [TestMethod]
        public void GetByOperation_Compare_ShouldReturnOnlyCompareRecords()
        {
            _repository.Save(new QuantityMeasurementEntity(
                "COMPARE", "1 FEET", "12 INCHES", "True", "LENGTH"));
            _repository.Save(new QuantityMeasurementEntity(
                "ADD", "1 FEET", "1 FEET", "2 FEET", "LENGTH"));
            _repository.Save(new QuantityMeasurementEntity(
                "COMPARE", "1 KG", "1000 GRAM", "True", "WEIGHT"));

            var result = _repository.GetByOperation("COMPARE");

            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.TrueForAll(e => e.Operation == "COMPARE"));
        }

        /// <summary>
        /// Filter by ADD operation should return only ADD records.
        /// </summary>
        [TestMethod]
        public void GetByOperation_Add_ShouldReturnOnlyAddRecords()
        {
            _repository.Save(new QuantityMeasurementEntity(
                "COMPARE", "1 FEET", "12 INCHES", "True", "LENGTH"));
            _repository.Save(new QuantityMeasurementEntity(
                "ADD", "1 FEET", "1 FEET", "2 FEET", "LENGTH"));

            var result = _repository.GetByOperation("ADD");

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("ADD", result[0].Operation);
        }

        /// <summary>
        /// Filter by operation with no matching records should return empty list.
        /// </summary>
        [TestMethod]
        public void GetByOperation_NoMatch_ShouldReturnEmptyList()
        {
            _repository.Save(new QuantityMeasurementEntity(
                "COMPARE", "1 FEET", "12 INCHES", "True", "LENGTH"));

            var result = _repository.GetByOperation("DIVIDE");

            Assert.AreEqual(0, result.Count);
        }

        /// <summary>
        /// Filter by CONVERT operation should return only CONVERT records.
        /// </summary>
        [TestMethod]
        public void GetByOperation_Convert_ShouldReturnOnlyConvertRecords()
        {
            _repository.Save(new QuantityMeasurementEntity(
                "CONVERT", "1 GALLON", "-", "3.78541 LITRE", "VOLUME"));
            _repository.Save(new QuantityMeasurementEntity(
                "COMPARE", "1 FEET", "12 INCHES", "True", "LENGTH"));

            var result = _repository.GetByOperation("CONVERT");

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("CONVERT", result[0].Operation);
        }

        // ─────────────────────────────────────────────────────────────────
        // GetByMeasureType Tests
        // ─────────────────────────────────────────────────────────────────

        /// <summary>
        /// Filter by LENGTH measure type should return only LENGTH records.
        /// </summary>
        [TestMethod]
        public void GetByMeasureType_Length_ShouldReturnOnlyLengthRecords()
        {
            _repository.Save(new QuantityMeasurementEntity(
                "COMPARE", "1 FEET", "12 INCHES", "True", "LENGTH"));
            _repository.Save(new QuantityMeasurementEntity(
                "ADD", "1 KG", "1000 GRAM", "2 KG", "WEIGHT"));
            _repository.Save(new QuantityMeasurementEntity(
                "CONVERT", "1 LITRE", "-", "1000 ML", "VOLUME"));

            var result = _repository.GetByMeasureType("LENGTH");

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("LENGTH", result[0].MeasureType);
        }

        /// <summary>
        /// Filter by WEIGHT measure type should return only WEIGHT records.
        /// </summary>
        [TestMethod]
        public void GetByMeasureType_Weight_ShouldReturnOnlyWeightRecords()
        {
            _repository.Save(new QuantityMeasurementEntity(
                "COMPARE", "1 FEET", "12 INCHES", "True", "LENGTH"));
            _repository.Save(new QuantityMeasurementEntity(
                "ADD", "1 KG", "1000 GRAM", "2 KG", "WEIGHT"));

            var result = _repository.GetByMeasureType("WEIGHT");

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("WEIGHT", result[0].MeasureType);
        }

        /// <summary>
        /// Filter by VOLUME measure type should return only VOLUME records.
        /// </summary>
        [TestMethod]
        public void GetByMeasureType_Volume_ShouldReturnOnlyVolumeRecords()
        {
            _repository.Save(new QuantityMeasurementEntity(
                "CONVERT", "1 LITRE", "-", "1000 ML", "VOLUME"));
            _repository.Save(new QuantityMeasurementEntity(
                "COMPARE", "1 FEET", "12 INCHES", "True", "LENGTH"));

            var result = _repository.GetByMeasureType("VOLUME");

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("VOLUME", result[0].MeasureType);
        }

        /// <summary>
        /// Filter by TEMPERATURE measure type should return only TEMPERATURE records.
        /// </summary>
        [TestMethod]
        public void GetByMeasureType_Temperature_ShouldReturnOnlyTemperatureRecords()
        {
            _repository.Save(new QuantityMeasurementEntity(
                "COMPARE", "100 CELSIUS", "212 FAHRENHEIT", "True", "TEMPERATURE"));
            _repository.Save(new QuantityMeasurementEntity(
                "COMPARE", "1 FEET", "12 INCHES", "True", "LENGTH"));

            var result = _repository.GetByMeasureType("TEMPERATURE");

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("TEMPERATURE", result[0].MeasureType);
        }

        /// <summary>
        /// Filter by measure type with no matching records should return empty list.
        /// </summary>
        [TestMethod]
        public void GetByMeasureType_NoMatch_ShouldReturnEmptyList()
        {
            _repository.Save(new QuantityMeasurementEntity(
                "COMPARE", "1 FEET", "12 INCHES", "True", "LENGTH"));

            var result = _repository.GetByMeasureType("TEMPERATURE");

            Assert.AreEqual(0, result.Count);
        }

        // ─────────────────────────────────────────────────────────────────
        // GetTotalCount Tests
        // ─────────────────────────────────────────────────────────────────

        /// <summary>
        /// Count on empty database should return zero.
        /// </summary>
        [TestMethod]
        public void GetTotalCount_EmptyDatabase_ShouldReturnZero()
        {
            Assert.AreEqual(0, _repository.GetTotalCount());
        }

        /// <summary>
        /// Count after saving one record should return one.
        /// </summary>
        [TestMethod]
        public void GetTotalCount_AfterSavingOne_ShouldReturnOne()
        {
            _repository.Save(new QuantityMeasurementEntity(
                "COMPARE", "1 FEET", "12 INCHES", "True", "LENGTH"));

            Assert.AreEqual(1, _repository.GetTotalCount());
        }

        /// <summary>
        /// Count after saving five records should return five.
        /// </summary>
        [TestMethod]
        public void GetTotalCount_AfterSavingFive_ShouldReturnFive()
        {
            for (int i = 0; i < 5; i++)
                _repository.Save(new QuantityMeasurementEntity(
                    "COMPARE", "1 FEET", "12 INCHES", "True", "LENGTH"));

            Assert.AreEqual(5, _repository.GetTotalCount());
        }

        // ─────────────────────────────────────────────────────────────────
        // DeleteAll Tests
        // ─────────────────────────────────────────────────────────────────

        /// <summary>
        /// DeleteAll should remove all records and return count of zero.
        /// </summary>
        [TestMethod]
        public void DeleteAll_AfterSavingRecords_ShouldReturnZeroCount()
        {
            _repository.Save(new QuantityMeasurementEntity(
                "COMPARE", "1 FEET", "12 INCHES", "True", "LENGTH"));
            _repository.Save(new QuantityMeasurementEntity(
                "ADD", "1 KG", "1000 GRAM", "2 KG", "WEIGHT"));

            _repository.DeleteAll();

            Assert.AreEqual(0, _repository.GetTotalCount());
        }

        /// <summary>
        /// DeleteAll on already empty database should not throw any exception.
        /// </summary>
        [TestMethod]
        public void DeleteAll_WhenAlreadyEmpty_ShouldNotThrowException()
        {
            _repository.DeleteAll();

            Assert.AreEqual(0, _repository.GetTotalCount());
        }

        // ─────────────────────────────────────────────────────────────────
        // GetPoolStats Tests
        // ─────────────────────────────────────────────────────────────────

        /// <summary>
        /// GetPoolStats should return a non empty string.
        /// </summary>
        [TestMethod]
        public void GetPoolStats_ShouldReturnNonEmptyString()
        {
            string stats = _repository.GetPoolStats();

            Assert.IsNotNull(stats);
            Assert.AreNotEqual("", stats);
        }

        /// <summary>
        /// GetPoolStats should contain database name in the result.
        /// </summary>
        [TestMethod]
        public void GetPoolStats_ShouldContainDatabaseName()
        {
            string stats = _repository.GetPoolStats();

            StringAssert.Contains(stats, "QuantityMeasurementDB");
        }

        // ─────────────────────────────────────────────────────────────────
        // SQL Injection Prevention Tests
        // ─────────────────────────────────────────────────────────────────

        /// <summary>
        /// SQL injection attempt in operation filter should return empty list
        /// and not drop or affect the database tables.
        /// </summary>
        [TestMethod]
        public void GetByOperation_SQLInjectionAttempt_ShouldReturnEmptyList()
        {
            _repository.Save(new QuantityMeasurementEntity(
                "COMPARE", "1 FEET", "12 INCHES", "True", "LENGTH"));

            var result = _repository.GetByOperation(
                "COMPARE'; DROP TABLE quantity_measurement_entity;--");

            Assert.AreEqual(0,  result.Count);
            Assert.AreEqual(1, _repository.GetTotalCount());
        }

        /// <summary>
        /// SQL injection attempt in measure type filter should return empty list
        /// and not drop or affect the database tables.
        /// </summary>
        [TestMethod]
        public void GetByMeasureType_SQLInjectionAttempt_ShouldReturnEmptyList()
        {
            _repository.Save(new QuantityMeasurementEntity(
                "COMPARE", "1 FEET", "12 INCHES", "True", "LENGTH"));

            var result = _repository.GetByMeasureType(
                "LENGTH'; DROP TABLE quantity_measurement_entity;--");

            Assert.AreEqual(0,  result.Count);
            Assert.AreEqual(1, _repository.GetTotalCount());
        }

        // ─────────────────────────────────────────────────────────────────
        // End to End Service with Database Repository Tests
        // ─────────────────────────────────────────────────────────────────

        /// <summary>
        /// Comparing 1 feet and 12 inches via service should save one record to database.
        /// </summary>
        [TestMethod]
        public void Integration_CompareLength_ShouldSaveToDatabase()
        {
            var first  = new QuantityDTO(1, LengthEnum.FEET);
            var second = new QuantityDTO(12, LengthEnum.INCHES);

            bool result = _service.Compare(first, second);

            Assert.IsTrue(result);
            Assert.AreEqual(1, _repository.GetTotalCount());
        }

        /// <summary>
        /// Comparing 1 kg and 1000 grams via service should save one record to database.
        /// </summary>
        [TestMethod]
        public void Integration_CompareWeight_ShouldSaveToDatabase()
        {
            var first  = new QuantityDTO(1, WeightEnum.KILOGRAM);
            var second = new QuantityDTO(1000, WeightEnum.GRAM);

            bool result = _service.Compare(first, second);

            Assert.IsTrue(result);
            Assert.AreEqual(1, _repository.GetTotalCount());
        }

        /// <summary>
        /// Adding 1 litre and 1000 millilitres via service should save one record to database.
        /// </summary>
        [TestMethod]
        public void Integration_AddVolume_ShouldSaveToDatabase()
        {
            var first  = new QuantityDTO(1, VolumeEnum.LITRE);
            var second = new QuantityDTO(1000, VolumeEnum.MILLILITRE);

            var result = _service.Add(first, second, VolumeEnum.LITRE);

            Assert.AreEqual(2, result.Value, 0.001);
            Assert.AreEqual(1, _repository.GetTotalCount());
        }

        /// <summary>
        /// Converting 1 feet to inches via service should save one record to database.
        /// </summary>
        [TestMethod]
        public void Integration_ConvertLength_ShouldSaveToDatabase()
        {
            var quantity = new QuantityDTO(1, LengthEnum.FEET);

            var result = _service.Convert(quantity, LengthEnum.INCHES);

            Assert.AreEqual(12, result.Value, 0.001);
            Assert.AreEqual(1, _repository.GetTotalCount());
        }

        /// <summary>
        /// Subtracting 1 feet from 2 feet via service should save one record to database.
        /// </summary>
        [TestMethod]
        public void Integration_SubtractLength_ShouldSaveToDatabase()
        {
            var first  = new QuantityDTO(2, LengthEnum.FEET);
            var second = new QuantityDTO(1, LengthEnum.FEET);

            var result = _service.Subtract(first, second);

            Assert.AreEqual(1, result.Value, 0.001);
            Assert.AreEqual(1, _repository.GetTotalCount());
        }

        /// <summary>
        /// Dividing 2 feet by 1 feet via service should save one record to database.
        /// </summary>
        [TestMethod]
        public void Integration_DivideLength_ShouldSaveToDatabase()
        {
            var first  = new QuantityDTO(2, LengthEnum.FEET);
            var second = new QuantityDTO(1, LengthEnum.FEET);

            double result = _service.Divide(first, second);

            Assert.AreEqual(2, result, 0.001);
            Assert.AreEqual(1, _repository.GetTotalCount());
        }

        /// <summary>
        /// Performing three different operations should save all three to database.
        /// </summary>
        [TestMethod]
        public void Integration_MultipleOperations_ShouldSaveAllToDatabase()
        {
            _service.Compare(
                new QuantityDTO(1, LengthEnum.FEET),
                new QuantityDTO(12, LengthEnum.INCHES));

            _service.Add(
                new QuantityDTO(1, WeightEnum.KILOGRAM),
                new QuantityDTO(1000, WeightEnum.GRAM),
                WeightEnum.KILOGRAM);

            _service.Convert(
                new QuantityDTO(1, VolumeEnum.LITRE),
                VolumeEnum.MILLILITRE);

            Assert.AreEqual(3, _repository.GetTotalCount());
        }

        /// <summary>
        /// GetByOperation after multiple operations should return correct filtered results.
        /// </summary>
        [TestMethod]
        public void Integration_GetByOperation_ShouldFilterCorrectly()
        {
            _service.Compare(
                new QuantityDTO(1, LengthEnum.FEET),
                new QuantityDTO(12, LengthEnum.INCHES));

            _service.Add(
                new QuantityDTO(1, LengthEnum.FEET),
                new QuantityDTO(1, LengthEnum.FEET),
                LengthEnum.FEET);

            var compareResults = _repository.GetByOperation("COMPARE");
            var addResults     = _repository.GetByOperation("ADD");

            Assert.AreEqual(1, compareResults.Count);
            Assert.AreEqual(1, addResults.Count);
        }

        /// <summary>
        /// GetByMeasureType after mixed operations should return correct filtered results.
        /// </summary>
        [TestMethod]
        public void Integration_GetByMeasureType_ShouldFilterCorrectly()
        {
            _service.Compare(
                new QuantityDTO(1, LengthEnum.FEET),
                new QuantityDTO(12, LengthEnum.INCHES));

            _service.Compare(
                new QuantityDTO(1, WeightEnum.KILOGRAM),
                new QuantityDTO(1000, WeightEnum.GRAM));

            var lengthResults = _repository.GetByMeasureType("LENGTH");
            var weightResults = _repository.GetByMeasureType("WEIGHT");

            Assert.AreEqual(1, lengthResults.Count);
            Assert.AreEqual(1, weightResults.Count);
        }

        /// <summary>
        /// DeleteAll via service should clear all records from database.
        /// </summary>
        [TestMethod]
        public void Integration_DeleteAll_ShouldClearAllRecords()
        {
            _service.Compare(
                new QuantityDTO(1, LengthEnum.FEET),
                new QuantityDTO(12, LengthEnum.INCHES));
            _service.Add(
                new QuantityDTO(1, WeightEnum.KILOGRAM),
                new QuantityDTO(1000, WeightEnum.GRAM),
                WeightEnum.KILOGRAM);

            _service.DeleteAll();

            Assert.AreEqual(0, _repository.GetTotalCount());
        }

        // ─────────────────────────────────────────────────────────────────
        // Cache Repository Independence Tests
        // ─────────────────────────────────────────────────────────────────

        /// <summary>
        /// Cache repository should work independently from database repository.
        /// Data saved to cache should not appear in database.
        /// </summary>
        [TestMethod]
        public void CacheRepository_ShouldWorkIndependentlyFromDatabase()
        {
            var cacheRepo = QuantityMeasurementCacheRepository.GetInstance();
            cacheRepo.DeleteAll();
            var cacheService = new QuantityMeasurementServiceImpl(cacheRepo);

            cacheService.Compare(
                new QuantityDTO(1, LengthEnum.FEET),
                new QuantityDTO(12, LengthEnum.INCHES));

            Assert.AreEqual(1, cacheRepo.GetTotalCount());
            Assert.AreEqual(0, _repository.GetTotalCount());
        }

        /// <summary>
        /// Both repositories should maintain their own separate data independently.
        /// </summary>
        [TestMethod]
        public void DatabaseAndCache_ShouldMaintainSeparateData()
        {
            var cacheRepo = QuantityMeasurementCacheRepository.GetInstance();
            cacheRepo.DeleteAll();
            var cacheService = new QuantityMeasurementServiceImpl(cacheRepo);

            cacheService.Compare(
                new QuantityDTO(1, LengthEnum.FEET),
                new QuantityDTO(12, LengthEnum.INCHES));

            _service.Compare(
                new QuantityDTO(1, WeightEnum.KILOGRAM),
                new QuantityDTO(1000, WeightEnum.GRAM));

            Assert.AreEqual(1, cacheRepo.GetTotalCount());
            Assert.AreEqual(1, _repository.GetTotalCount());
        }
    }
}