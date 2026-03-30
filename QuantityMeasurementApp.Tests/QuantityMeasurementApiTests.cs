using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.BusinessLayer.Services;
using QuantityMeasurementApp.ModelLayer.DTO;
using QuantityMeasurementApp.ModelLayer.Enums;
using QuantityMeasurementApp.RepoLayer.Repositories;
using System.Collections.Generic;

namespace QuantityMeasurementApp.Tests
{

    [TestClass]
    public class QuantityMeasurementApiTests
    {
        //  Fields
        private InMemoryQuantityRepository     _repository;
        private QuantityMeasurementServiceImpl _service;
        private EncryptionService              _encryptionService;

        //  Setup and Teardown 

        [TestInitialize]
        public void SetUp()
        {
            _repository   = new InMemoryQuantityRepository();
            var logger    = NullLogger<QuantityMeasurementServiceImpl>.Instance;
            _service      = new QuantityMeasurementServiceImpl(_repository, logger);

            // Encryption service with test key
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "Encryption:Key", "TestKey-AES256-32CharactersLong!!" }
                })
                .Build();
            _encryptionService = new EncryptionService(config,
                NullLogger<EncryptionService>.Instance);
        }

        [TestCleanup]
        public void TearDown() => _repository.DeleteAll();

        //  Compare

        /// <summary>1 FEET equals 12 INCHES should return true.</summary>
        [TestMethod]
        public void Compare_OneFeet_TwelveInches_ReturnsTrue()
        {
            var result = _service.Compare(
                new QuantityDTO(1.0,  LengthEnum.FEET),
                new QuantityDTO(12.0, LengthEnum.INCHES));

            Assert.AreEqual("true",    result.Result);
            Assert.AreEqual("COMPARE", result.Operation);
            Assert.AreEqual("LENGTH",  result.MeasureType);
            Assert.IsFalse(result.IsError);
        }

        /// <summary>Different length values should return false.</summary>
        [TestMethod]
        public void Compare_DifferentLengthValues_ReturnsFalse()
        {
            var result = _service.Compare(
                new QuantityDTO(1.0, LengthEnum.FEET),
                new QuantityDTO(2.0, LengthEnum.FEET));

            Assert.AreEqual("false", result.Result);
        }

        /// <summary>1 KILOGRAM equals 1000 GRAM should return true.</summary>
        [TestMethod]
        public void Compare_OneKilogram_ThousandGram_ReturnsTrue()
        {
            var result = _service.Compare(
                new QuantityDTO(1.0,    WeightEnum.KILOGRAM),
                new QuantityDTO(1000.0, WeightEnum.GRAM));

            Assert.AreEqual("true",   result.Result);
            Assert.AreEqual("WEIGHT", result.MeasureType);
        }

        /// <summary>1 LITRE equals 1000 MILLILITRE should return true.</summary>
        [TestMethod]
        public void Compare_OneLitre_ThousandMillilitre_ReturnsTrue()
        {
            var result = _service.Compare(
                new QuantityDTO(1.0,    VolumeEnum.LITRE),
                new QuantityDTO(1000.0, VolumeEnum.MILLILITRE));

            Assert.AreEqual("true",   result.Result);
            Assert.AreEqual("VOLUME", result.MeasureType);
        }

        /// <summary>Compare should save exactly one record to repository.</summary>
        [TestMethod]
        public void Compare_ShouldPersistOneRecordToRepository()
        {
            _service.Compare(
                new QuantityDTO(1.0,  LengthEnum.FEET),
                new QuantityDTO(12.0, LengthEnum.INCHES));

            Assert.AreEqual(1, _repository.GetTotalCount());
        }

        //  Convert 

        /// <summary>1 FEET converted to INCHES should give 12.</summary>
        [TestMethod]
        public void Convert_OneFeet_ToInches_ReturnsTwelve()
        {
            var result = _service.Convert(
                new QuantityDTO(1.0, LengthEnum.FEET),
                LengthEnum.INCHES);

            Assert.IsTrue(result.Result.Contains("12"));
            Assert.AreEqual("CONVERT", result.Operation);
        }

        /// <summary>1 KILOGRAM converted to GRAM should give 1000.</summary>
        [TestMethod]
        public void Convert_OneKilogram_ToGram_ReturnsThousand()
        {
            var result = _service.Convert(
                new QuantityDTO(1.0, WeightEnum.KILOGRAM),
                WeightEnum.GRAM);

            Assert.IsTrue(result.Result.Contains("1000"));
        }

        /// <summary>1 LITRE converted to MILLILITRE should give 1000.</summary>
        [TestMethod]
        public void Convert_OneLitre_ToMillilitre_ReturnsThousand()
        {
            var result = _service.Convert(
                new QuantityDTO(1.0, VolumeEnum.LITRE),
                VolumeEnum.MILLILITRE);

            Assert.IsTrue(result.Result.Contains("1000"));
        }

        /// <summary>100 CELSIUS converted to FAHRENHEIT should give 212.</summary>
        [TestMethod]
        public void Convert_HundredCelsius_ToFahrenheit_ReturnsTwoTwelve()
        {
            var result = _service.Convert(
                new QuantityDTO(100.0, TemperatureEnum.CELSIUS),
                TemperatureEnum.FAHRENHEIT);

            Assert.IsTrue(result.Result.Contains("212"));
        }

        //  Add

        /// <summary>1 FEET plus 12 INCHES should equal 2 FEET.</summary>
        [TestMethod]
        public void Add_OneFeet_TwelveInches_ReturnsTwoFeet()
        {
            var result = _service.Add(
                new QuantityDTO(1.0,  LengthEnum.FEET),
                new QuantityDTO(12.0, LengthEnum.INCHES),
                LengthEnum.FEET);

            Assert.IsTrue(result.Result.Contains("2"));
            Assert.AreEqual("ADD", result.Operation);
        }

        /// <summary>1 KILOGRAM plus 1000 GRAM should equal 2 KILOGRAM.</summary>
        [TestMethod]
        public void Add_OneKilogram_ThousandGram_ReturnsTwoKilogram()
        {
            var result = _service.Add(
                new QuantityDTO(1.0,    WeightEnum.KILOGRAM),
                new QuantityDTO(1000.0, WeightEnum.GRAM),
                WeightEnum.KILOGRAM);

            Assert.IsTrue(result.Result.Contains("2"));
        }

        /// <summary>1 LITRE plus 1000 MILLILITRE should equal 2 LITRE.</summary>
        [TestMethod]
        public void Add_OneLitre_ThousandMillilitre_ReturnsTwoLitre()
        {
            var result = _service.Add(
                new QuantityDTO(1.0,    VolumeEnum.LITRE),
                new QuantityDTO(1000.0, VolumeEnum.MILLILITRE),
                VolumeEnum.LITRE);

            Assert.IsTrue(result.Result.Contains("2"));
        }

        /// <summary>Temperature add is not supported and should save error record.</summary>
        [TestMethod]
        public void Add_TemperatureOperation_SavesErrorRecord()
        {
            var result = _service.Add(
                new QuantityDTO(100.0, TemperatureEnum.CELSIUS),
                new QuantityDTO(50.0,  TemperatureEnum.CELSIUS),
                TemperatureEnum.CELSIUS);

            Assert.IsTrue(result.IsError);
            Assert.IsNotNull(result.ErrorMessage);
        }

        //  Subtract 

        /// <summary>2 FEET minus 1 FEET should equal 1 FEET.</summary>
        [TestMethod]
        public void Subtract_TwoFeet_OneFeet_ReturnsOneFeet()
        {
            var result = _service.Subtract(
                new QuantityDTO(2.0, LengthEnum.FEET),
                new QuantityDTO(1.0, LengthEnum.FEET));

            Assert.IsTrue(result.Result.Contains("1"));
            Assert.AreEqual("SUBTRACT", result.Operation);
        }

        /// <summary>2 KILOGRAM minus 500 GRAM should equal 1.5 KILOGRAM.</summary>
        [TestMethod]
        public void Subtract_TwoKilogram_FiveHundredGram_ReturnsOnePointFive()
        {
            var result = _service.Subtract(
                new QuantityDTO(2.0,   WeightEnum.KILOGRAM),
                new QuantityDTO(500.0, WeightEnum.GRAM));

            Assert.IsTrue(result.Result.Contains("1.5"));
        }

        //  Divide 

        /// <summary>2 FEET divided by 1 FEET should return ratio 2.</summary>
        [TestMethod]
        public void Divide_TwoFeet_OneFeet_ReturnsTwoRatio()
        {
            var result = _service.Divide(
                new QuantityDTO(2.0, LengthEnum.FEET),
                new QuantityDTO(1.0, LengthEnum.FEET));

            Assert.IsTrue(result.Result.Contains("2"));
            Assert.AreEqual("DIVIDE", result.Operation);
        }

        /// <summary>2 LITRE divided by 1 LITRE should return ratio 2.</summary>
        [TestMethod]
        public void Divide_TwoLitre_OneLitre_ReturnsTwoRatio()
        {
            var result = _service.Divide(
                new QuantityDTO(2.0, VolumeEnum.LITRE),
                new QuantityDTO(1.0, VolumeEnum.LITRE));

            Assert.IsTrue(result.Result.Contains("2"));
        }

        //  History 

        /// <summary>History should return all 3 records after 3 operations.</summary>
        [TestMethod]
        public void GetAllMeasurements_AfterThreeOperations_ReturnsThree()
        {
            _service.Compare(new QuantityDTO(1.0, LengthEnum.FEET),    new QuantityDTO(12.0,   LengthEnum.INCHES));
            _service.Add(    new QuantityDTO(1.0, WeightEnum.KILOGRAM), new QuantityDTO(1000.0, WeightEnum.GRAM), WeightEnum.KILOGRAM);
            _service.Convert(new QuantityDTO(1.0, VolumeEnum.LITRE),    VolumeEnum.MILLILITRE);

            Assert.AreEqual(3, _service.GetAllMeasurements().Count);
        }

        /// <summary>Filter by COMPARE should return only compare records.</summary>
        [TestMethod]
        public void GetByOperation_Compare_ReturnsOnlyCompareRecords()
        {
            _service.Compare(new QuantityDTO(1.0, LengthEnum.FEET), new QuantityDTO(12.0, LengthEnum.INCHES));
            _service.Add(    new QuantityDTO(1.0, LengthEnum.FEET), new QuantityDTO(1.0,  LengthEnum.FEET), LengthEnum.FEET);

            var records = _service.GetByOperation("COMPARE");
            Assert.AreEqual(1,         records.Count);
            Assert.AreEqual("COMPARE", records[0].Operation);
        }

        /// <summary>Filter by LENGTH type should return only length records.</summary>
        [TestMethod]
        public void GetByMeasureType_Length_ReturnsOnlyLengthRecords()
        {
            _service.Compare(new QuantityDTO(1.0, LengthEnum.FEET),    new QuantityDTO(12.0,   LengthEnum.INCHES));
            _service.Compare(new QuantityDTO(1.0, WeightEnum.KILOGRAM), new QuantityDTO(1000.0, WeightEnum.GRAM));

            var records = _service.GetByMeasureType("LENGTH");
            Assert.AreEqual(1,        records.Count);
            Assert.AreEqual("LENGTH", records[0].MeasureType);
        }

        /// <summary>GetErrored should return only error records.</summary>
        [TestMethod]
        public void GetErrored_AfterFailedOperation_ReturnsErrorRecords()
        {
            _service.Add(
                new QuantityDTO(100.0, TemperatureEnum.CELSIUS),
                new QuantityDTO(50.0,  TemperatureEnum.CELSIUS),
                TemperatureEnum.CELSIUS);

            var errored = _service.GetErrored();
            Assert.IsTrue(errored.Count > 0);
            Assert.IsTrue(errored[0].IsError);
        }

        /// <summary>GetTotalCount should return correct count after 5 operations.</summary>
        [TestMethod]
        public void GetTotalCount_AfterFiveOperations_ReturnsFive()
        {
            for (int i = 0; i < 5; i++)
                _service.Compare(
                    new QuantityDTO(1.0,  LengthEnum.FEET),
                    new QuantityDTO(12.0, LengthEnum.INCHES));

            Assert.AreEqual(5, _service.GetTotalCount());
        }

        /// <summary>DeleteAll should clear all records from repository.</summary>
        [TestMethod]
        public void DeleteAll_ClearsAllRecords()
        {
            _service.Compare(new QuantityDTO(1.0, LengthEnum.FEET), new QuantityDTO(12.0, LengthEnum.INCHES));
            _service.DeleteAll();
            Assert.AreEqual(0, _service.GetTotalCount());
        }

        //  AES Encryption

        /// <summary>Encrypting plain text should return non-empty Base64 string.</summary>
        [TestMethod]
        public void Encrypt_PlainText_ReturnsNonEmptyBase64()
        {
            string encrypted = _encryptionService.Encrypt("Hello World");

            Assert.IsNotNull(encrypted);
            Assert.AreNotEqual("Hello World", encrypted);
        }

        /// <summary>Decrypting cipher text should return original plain text.</summary>
        [TestMethod]
        public void Decrypt_CipherText_ReturnsOriginalText()
        {
            string original  = "Hello World";
            string encrypted = _encryptionService.Encrypt(original);
            string decrypted = _encryptionService.Decrypt(encrypted);

            Assert.AreEqual(original, decrypted);
        }

        /// <summary>Encrypting same text twice should give different cipher each time.</summary>
        [TestMethod]
        public void Encrypt_SameTextTwice_ReturnsDifferentCipher()
        {
            string encrypted1 = _encryptionService.Encrypt("Test");
            string encrypted2 = _encryptionService.Encrypt("Test");

            Assert.AreNotEqual(encrypted1, encrypted2);
        }

        /// <summary>Encrypting empty string should return empty string.</summary>
        [TestMethod]
        public void Encrypt_EmptyString_ReturnsEmptyString()
        {
            Assert.AreEqual(string.Empty, _encryptionService.Encrypt(string.Empty));
        }

        /// <summary>Full flow: encrypt then decrypt returns original.</summary>
        [TestMethod]
        public void EncryptThenDecrypt_ReturnsOriginalText()
        {
            string original  = "SensitiveData@123";
            string encrypted = _encryptionService.Encrypt(original);
            string decrypted = _encryptionService.Decrypt(encrypted);

            Assert.AreEqual(original, decrypted);
        }

        //  BCrypt 

        /// <summary>Same password hashed twice must give different hashes.</summary>
        [TestMethod]
        public void BCrypt_SamePassword_ProducesDifferentHashes()
        {
            string hash1 = BCrypt.Net.BCrypt.HashPassword("Test@1234", workFactor: 12);
            string hash2 = BCrypt.Net.BCrypt.HashPassword("Test@1234", workFactor: 12);

            Assert.AreNotEqual(hash1, hash2);
        }

        /// <summary>Correct password must verify successfully against its hash.</summary>
        [TestMethod]
        public void BCrypt_CorrectPassword_VerifiesSuccessfully()
        {
            string password = "Test@1234";
            string hash     = BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);

            Assert.IsTrue(BCrypt.Net.BCrypt.Verify(password, hash));
        }

        /// <summary>Wrong password must fail verification against stored hash.</summary>
        [TestMethod]
        public void BCrypt_WrongPassword_FailsVerification()
        {
            string hash = BCrypt.Net.BCrypt.HashPassword("Test@1234", workFactor: 12);

            Assert.IsFalse(BCrypt.Net.BCrypt.Verify("WrongPassword@999", hash));
        }

        /// <summary>Empty password must fail verification.</summary>
        [TestMethod]
        public void BCrypt_EmptyPassword_FailsVerification()
        {
            string hash = BCrypt.Net.BCrypt.HashPassword("Test@1234", workFactor: 12);

            Assert.IsFalse(BCrypt.Net.BCrypt.Verify(string.Empty, hash));
        }
    }
}