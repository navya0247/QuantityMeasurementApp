using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.ModelLayer.Models;
using QuantityMeasurementApp.BusinessLayer.Services;
using System;

namespace QuantityMeasurementApp.Tests
{
    /// <summary>
    /// UC14
    /// Test cases for Temperature measurement.
    /// Verifies temperature equality, conversion, and unsupported arithmetic operations.
    /// </summary>
    [TestClass]
    public class TemperatureMeasurementTests
    {
        /// <summary>
        /// Verify Celsius to Fahrenheit equality.
        /// 0°C should equal 32°F.
        /// </summary>
        [TestMethod]
        public void TemperatureEquality_CelsiusToFahrenheit()
        {
            var t1 = new Quantity<TemperatureEnum>(0, TemperatureEnum.CELSIUS);
            var t2 = new Quantity<TemperatureEnum>(32, TemperatureEnum.FAHRENHEIT);

            Assert.IsTrue(t1.Equals(t2));
        }

        /// <summary>
        /// Verify Celsius to Kelvin equality.
        /// 0°C should equal 273.15 K.
        /// </summary>
        [TestMethod]
        public void TemperatureEquality_CelsiusToKelvin()
        {
            var t1 = new Quantity<TemperatureEnum>(0, TemperatureEnum.CELSIUS);
            var t2 = new Quantity<TemperatureEnum>(273.15, TemperatureEnum.KELVIN);

            Assert.IsTrue(t1.Equals(t2));
        }

        /// <summary>
        /// Verify conversion from Celsius to Fahrenheit.
        /// 100°C should convert to 212°F.
        /// </summary>
        [TestMethod]
        public void TemperatureConversion_CelsiusToFahrenheit()
        {
            var temp = new Quantity<TemperatureEnum>(100, TemperatureEnum.CELSIUS);

            var result = temp.ConvertTo(TemperatureEnum.FAHRENHEIT);

            Assert.AreEqual(212, result.Value, 0.01);
        }

        /// <summary>
        /// Verify conversion from Fahrenheit to Celsius.
        /// 32°F should convert to 0°C.
        /// </summary>
        [TestMethod]
        public void TemperatureConversion_FahrenheitToCelsius()
        {
            var temp = new Quantity<TemperatureEnum>(32, TemperatureEnum.FAHRENHEIT);

            var result = temp.ConvertTo(TemperatureEnum.CELSIUS);

            Assert.AreEqual(0, result.Value, 0.01);
        }

        /// <summary>
        /// Verify conversion from Celsius to Kelvin.
        /// </summary>
        [TestMethod]
        public void TemperatureConversion_CelsiusToKelvin()
        {
            var temp = new Quantity<TemperatureEnum>(0, TemperatureEnum.CELSIUS);

            var result = temp.ConvertTo(TemperatureEnum.KELVIN);

            Assert.AreEqual(273.15, result.Value, 0.01);
        }

        /// <summary>
        /// Verify unsupported addition operation for temperature.
        /// </summary>
        [TestMethod]
        public void Temperature_Addition_ShouldThrowException()
        {
            var t1 = new Quantity<TemperatureEnum>(100, TemperatureEnum.CELSIUS);
            var t2 = new Quantity<TemperatureEnum>(50, TemperatureEnum.CELSIUS);

            try
            {
                t1.Add(t2, TemperatureEnum.CELSIUS);
                Assert.Fail("Expected NotSupportedException was not thrown.");
            }
            catch (NotSupportedException)
            {
                Assert.IsTrue(true);
            }
        }

        /// <summary>
        /// Verify unsupported subtraction operation for temperature.
        /// </summary>
        [TestMethod]
        public void Temperature_Subtraction_ShouldThrowException()
        {
            var t1 = new Quantity<TemperatureEnum>(100, TemperatureEnum.CELSIUS);
            var t2 = new Quantity<TemperatureEnum>(50, TemperatureEnum.CELSIUS);

            try
            {
                t1.Subtract(t2);
                Assert.Fail("Expected NotSupportedException was not thrown.");
            }
            catch (NotSupportedException)
            {
                Assert.IsTrue(true);
            }
        }

        /// <summary>
        /// Verify unsupported division operation for temperature.
        /// </summary>
        [TestMethod]
        public void Temperature_Division_ShouldThrowException()
        {
            var t1 = new Quantity<TemperatureEnum>(100, TemperatureEnum.CELSIUS);
            var t2 = new Quantity<TemperatureEnum>(50, TemperatureEnum.CELSIUS);

            try
            {
                t1.Divide(t2);
                Assert.Fail("Expected NotSupportedException was not thrown.");
            }
            catch (NotSupportedException)
            {
                Assert.IsTrue(true);
            }
        }
        /// <summary>
        /// Verify temperature cannot be compared with length.
        /// </summary>
        [TestMethod]
        public void TemperatureVsLength_ShouldReturnFalse()
        {
            var temp = new Quantity<TemperatureEnum>(50, TemperatureEnum.CELSIUS);
            var length = new Quantity<LengthEnum>(50, LengthEnum.FEET);

            Assert.IsFalse(temp.Equals(length));
        }
    }
}