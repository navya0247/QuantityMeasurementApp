using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Services;

namespace QuantityMeasurementApp.Tests
{
    /// <summary>
    /// UC10 Tests
    /// Updated to use Generic Quantity<U> class
    /// </summary>
    [TestClass]
    public class QuantityMeasurementTests
    {
        // ---------- UC1 ----------
        [TestMethod]
        public void FeetEquality_SameValue_ReturnsTrue()
        {
            Assert.IsTrue(QuantityMeasurementService.AreFeetEqual(1.0, 1.0));
        }

        [TestMethod]
        public void FeetEquality_DifferentValue_ReturnsFalse()
        {
            Assert.IsFalse(QuantityMeasurementService.AreFeetEqual(1.0, 2.0));
        }

        // ---------- UC2 ----------
        [TestMethod]
        public void InchEquality_SameValue_ReturnsTrue()
        {
            Assert.IsTrue(QuantityMeasurementService.AreInchesEqual(5.0, 5.0));
        }

        [TestMethod]
        public void InchEquality_DifferentValue_ReturnsFalse()
        {
            Assert.IsFalse(QuantityMeasurementService.AreInchesEqual(5.0, 6.0));
        }

        // ---------- UC3 ----------
        [TestMethod]
        public void FeetToFeet_SameValue_ReturnsTrue()
        {
            Assert.IsTrue(
                QuantityMeasurementService.AreLengthEqual(
                    1.0, LengthUnit.FEET,
                    1.0, LengthUnit.FEET));
        }

        [TestMethod]
        public void FeetToInches_Equivalent_ReturnsTrue()
        {
            Assert.IsTrue(
                QuantityMeasurementService.AreLengthEqual(
                    1.0, LengthUnit.FEET,
                    12.0, LengthUnit.INCHES));
        }

        // ---------- UC4 ----------
        [TestMethod]
        public void YardToFeet_Equivalent_ReturnsTrue()
        {
            Assert.IsTrue(
                QuantityMeasurementService.AreLengthEqual(
                    1.0, LengthUnit.YARDS,
                    3.0, LengthUnit.FEET));
        }

        [TestMethod]
        public void CmToInch_Equivalent_ReturnsTrue()
        {
            Assert.IsTrue(
                QuantityMeasurementService.AreLengthEqual(
                    2.54, LengthUnit.CENTIMETERS,
                    1.0, LengthUnit.INCHES));
        }

        // ---------- UC5 ----------
        [TestMethod]
        public void Convert_FeetToInches_Returns12()
        {
            double result = QuantityMeasurementService.ConvertLength(
                1.0, LengthUnit.FEET, LengthUnit.INCHES);

            Assert.AreEqual(12.0, result, 0.0001);
        }

        [TestMethod]
        public void Convert_InchesToFeet_Returns2()
        {
            double result = QuantityMeasurementService.ConvertLength(
                24.0, LengthUnit.INCHES, LengthUnit.FEET);

            Assert.AreEqual(2.0, result, 0.0001);
        }

        // ---------- UC6 ----------
        [TestMethod]
        public void Add_FeetPlusFeet_Returns3Feet()
        {
            var result = QuantityMeasurementService.AddLengths(
                1.0, LengthUnit.FEET,
                2.0, LengthUnit.FEET);

            Assert.AreEqual(3.0, result.Value, 0.0001);
        }

        [TestMethod]
        public void Add_FeetPlusInches_Returns2Feet()
        {
            var result = QuantityMeasurementService.AddLengths(
                1.0, LengthUnit.FEET,
                12.0, LengthUnit.INCHES);

            Assert.AreEqual(2.0, result.Value, 0.0001);
        }

        // ---------- UC7 ----------
        [TestMethod]
        public void Add_WithTargetUnit_Inches()
        {
            var result = QuantityMeasurementService.AddLengths(
                1.0, LengthUnit.FEET,
                12.0, LengthUnit.INCHES,
                LengthUnit.INCHES);

            Assert.AreEqual(24.0, result.Value, 0.0001);
        }

        [TestMethod]
        public void Add_WithTargetUnit_Yards()
        {
            var result = QuantityMeasurementService.AddLengths(
                1.0, LengthUnit.FEET,
                12.0, LengthUnit.INCHES,
                LengthUnit.YARDS);

            Assert.AreEqual(0.6667, result.Value, 0.0001);
        }

        // ---------- UC8 ----------
        [TestMethod]
        public void LengthUnit_ConvertToBaseUnit()
        {
            double result = LengthUnit.INCHES.ConvertToBaseUnit(12);

            Assert.AreEqual(1.0, result, 0.0001);
        }

        [TestMethod]
        public void LengthUnit_ConvertFromBaseUnit()
        {
            double result = LengthUnit.INCHES.ConvertFromBaseUnit(1);

            Assert.AreEqual(12.0, result, 0.0001);
        }

        // ---------- UC10 ----------
        [TestMethod]
        public void GenericQuantity_LengthEquality()
        {
            var q1 = new Quantity<LengthUnit>(1.0, LengthUnit.FEET);
            var q2 = new Quantity<LengthUnit>(12.0, LengthUnit.INCHES);

            Assert.IsTrue(q1.Equals(q2));
        }

        [TestMethod]
        public void GenericQuantity_LengthConversion()
        {
            var q = new Quantity<LengthUnit>(1.0, LengthUnit.FEET);

            var result = q.ConvertTo(LengthUnit.INCHES);

            Assert.AreEqual(12.0, result.Value, 0.0001);
        }

        [TestMethod]
        public void GenericQuantity_LengthAddition()
        {
            var q1 = new Quantity<LengthUnit>(1.0, LengthUnit.FEET);
            var q2 = new Quantity<LengthUnit>(12.0, LengthUnit.INCHES);

            var result = q1.Add(q2, LengthUnit.FEET);

            Assert.AreEqual(2.0, result.Value, 0.0001);
        }
    }
}