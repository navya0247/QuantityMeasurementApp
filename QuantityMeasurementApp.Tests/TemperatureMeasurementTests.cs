using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Models;
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
            var t1 = new Quantity<TemperatureUnit>(0, TemperatureUnit.CELSIUS);
            var t2 = new Quantity<TemperatureUnit>(32, TemperatureUnit.FAHRENHEIT);

            Assert.IsTrue(t1.Equals(t2));
        }

        /// <summary>
        /// Verify Celsius to Kelvin equality.
        /// 0°C should equal 273.15 K.
        /// </summary>
        [TestMethod]
        public void TemperatureEquality_CelsiusToKelvin()
        {
            var t1 = new Quantity<TemperatureUnit>(0, TemperatureUnit.CELSIUS);
            var t2 = new Quantity<TemperatureUnit>(273.15, TemperatureUnit.KELVIN);

            Assert.IsTrue(t1.Equals(t2));
        }

        /// <summary>
        /// Verify conversion from Celsius to Fahrenheit.
        /// 100°C should convert to 212°F.
        /// </summary>
        [TestMethod]
        public void TemperatureConversion_CelsiusToFahrenheit()
        {
            var temp = new Quantity<TemperatureUnit>(100, TemperatureUnit.CELSIUS);

            var result = temp.ConvertTo(TemperatureUnit.FAHRENHEIT);

            Assert.AreEqual(212, result.Value, 0.01);
        }

        /// <summary>
        /// Verify conversion from Fahrenheit to Celsius.
        /// 32°F should convert to 0°C.
        /// </summary>
        [TestMethod]
        public void TemperatureConversion_FahrenheitToCelsius()
        {
            var temp = new Quantity<TemperatureUnit>(32, TemperatureUnit.FAHRENHEIT);

            var result = temp.ConvertTo(TemperatureUnit.CELSIUS);

            Assert.AreEqual(0, result.Value, 0.01);
        }

        /// <summary>
        /// Verify conversion from Celsius to Kelvin.
        /// </summary>
        [TestMethod]
        public void TemperatureConversion_CelsiusToKelvin()
        {
            var temp = new Quantity<TemperatureUnit>(0, TemperatureUnit.CELSIUS);

            var result = temp.ConvertTo(TemperatureUnit.KELVIN);

            Assert.AreEqual(273.15, result.Value, 0.01);
        }

        /// <summary>
        /// Verify unsupported addition operation for temperature.
        /// </summary>
        [TestMethod]
        public void Temperature_Addition_ShouldThrowException()
        {
            var t1 = new Quantity<TemperatureUnit>(100, TemperatureUnit.CELSIUS);
            var t2 = new Quantity<TemperatureUnit>(50, TemperatureUnit.CELSIUS);

            try
            {
                t1.Add(t2, TemperatureUnit.CELSIUS);
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
            var t1 = new Quantity<TemperatureUnit>(100, TemperatureUnit.CELSIUS);
            var t2 = new Quantity<TemperatureUnit>(50, TemperatureUnit.CELSIUS);

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
            var t1 = new Quantity<TemperatureUnit>(100, TemperatureUnit.CELSIUS);
            var t2 = new Quantity<TemperatureUnit>(50, TemperatureUnit.CELSIUS);

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
            var temp = new Quantity<TemperatureUnit>(50, TemperatureUnit.CELSIUS);
            var length = new Quantity<LengthUnit>(50, LengthUnit.FEET);

            Assert.IsFalse(temp.Equals(length));
        }
    }
}