using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Services;

namespace QuantityMeasurementApp.Tests
{
    [TestClass]
    public class QuantityMeasurementTests
    {
        // ================= UC1 – FEET =================

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

        [TestMethod]
        public void FeetEquality_SameReference_ReturnsTrue()
        {
            Feet f = new Feet(1.0);
            Assert.IsTrue(f.Equals(f));
        }

        [TestMethod]
        public void FeetEquality_NullComparison_ReturnsFalse()
        {
            Feet f = new Feet(1.0);
            Assert.IsFalse(f.Equals(null));
        }

        // ================= UC2 – INCHES =================

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

        [TestMethod]
        public void InchEquality_SameReference_ReturnsTrue()
        {
            Inches i = new Inches(2.0);
            Assert.IsTrue(i.Equals(i));
        }

        [TestMethod]
        public void InchEquality_NullComparison_ReturnsFalse()
        {
            Inches i = new Inches(2.0);
            Assert.IsFalse(i.Equals(null));
        }

        // ================= UC3 – GENERIC LENGTH =================

        [TestMethod]
        public void FeetToFeet_SameValue_ReturnsTrue()
        {
            Assert.IsTrue(QuantityMeasurementService.AreLengthEqual(1.0, LengthUnit.FEET, 1.0, LengthUnit.FEET));
        }

        [TestMethod]
        public void InchToInch_SameValue_ReturnsTrue()
        {
            Assert.IsTrue(QuantityMeasurementService.AreLengthEqual(5.0, LengthUnit.INCHES, 5.0, LengthUnit.INCHES));
        }

        [TestMethod]
        public void FeetToInch_Equivalent_ReturnsTrue()
        {
            Assert.IsTrue(QuantityMeasurementService.AreLengthEqual(1.0, LengthUnit.FEET, 12.0, LengthUnit.INCHES));
        }

        [TestMethod]
        public void InchToFeet_Equivalent_ReturnsTrue()
        {
            Assert.IsTrue(QuantityMeasurementService.AreLengthEqual(12.0, LengthUnit.INCHES, 1.0, LengthUnit.FEET));
        }

        [TestMethod]
        public void DifferentValues_ReturnsFalse()
        {
            Assert.IsFalse(QuantityMeasurementService.AreLengthEqual(1.0, LengthUnit.FEET, 2.0, LengthUnit.FEET));
        }

        // ================= UC4 – YARDS + CM =================

        [TestMethod]
        public void YardToFeet_Equivalent_ReturnsTrue()
        {
            Assert.IsTrue(
                QuantityMeasurementService.AreLengthEqual(1.0, LengthUnit.YARDS, 3.0, LengthUnit.FEET));
        }

        [TestMethod]
        public void YardToInches_Equivalent_ReturnsTrue()
        {
            Assert.IsTrue(
                QuantityMeasurementService.AreLengthEqual(1.0, LengthUnit.YARDS, 36.0, LengthUnit.INCHES));
        }

        [TestMethod]
        public void CmToInch_Equivalent_ReturnsTrue()
        {
            Assert.IsTrue(
                QuantityMeasurementService.AreLengthEqual(1.0, LengthUnit.CENTIMETERS, 0.393701, LengthUnit.INCHES));
        }

        // ================= UC5 – CONVERSION =================

        [TestMethod]
        public void Convert_FeetToInches_Returns()
        {
            double result = QuantityMeasurementService.ConvertLength(1.0, LengthUnit.FEET, LengthUnit.INCHES);
            Assert.AreEqual(12.0, result, 0.0001);
        }

        [TestMethod]
        public void Convert_InchesToFeet_Returns()
        {
            double result = QuantityMeasurementService.ConvertLength(24.0, LengthUnit.INCHES, LengthUnit.FEET);
            Assert.AreEqual(2.0, result, 0.0001);
        }

        [TestMethod]
        public void Convert_YardToFeet_Returns()
        {
            double result = QuantityMeasurementService.ConvertLength(1.0, LengthUnit.YARDS, LengthUnit.FEET);
            Assert.AreEqual(3.0, result, 0.0001);
        }

        [TestMethod]
        public void Convert_CmToInches_Returns()
        {
            double result = QuantityMeasurementService.ConvertLength(2.54, LengthUnit.CENTIMETERS, LengthUnit.INCHES);
            Assert.AreEqual(1.0, result, 0.0001);
        }

        [TestMethod]
        public void Convert_SameUnit_ReturnsSameValue()
        {
            double result = QuantityMeasurementService.ConvertLength(5.0, LengthUnit.FEET, LengthUnit.FEET);
           Assert.AreEqual(5.0, result, 0.0001);
        }

        [TestMethod]
        public void Convert_ZeroValue_ReturnsZero()
        {
            double result = QuantityMeasurementService.ConvertLength(0.0, LengthUnit.FEET, LengthUnit.INCHES);
            Assert.AreEqual(0.0, result);
        }

        [TestMethod]
        public void Convert_NegativeValue_ReturnsCorrect()
        {
            double result = QuantityMeasurementService.ConvertLength(-1.0, LengthUnit.FEET, LengthUnit.INCHES);
            Assert.AreEqual(-12.0, result);
        }

        // ---------------- UC6 ADDITION TESTS ----------------

        // Same unit addition
        [TestMethod]
        public void Add_FeetPlusFeet_ReturnsFeet()
        {
            var result = QuantityMeasurementService.AddLengths(
                1.0, LengthUnit.FEET,
                2.0, LengthUnit.FEET);

            Assert.AreEqual(3.0, result.Value, 0.0001);
        }

        // Cross unit addition
        [TestMethod]
        public void Add_FeetPlusInches_ReturnsFeet()
        {
            var result = QuantityMeasurementService.AddLengths(
                1.0, LengthUnit.FEET,
                12.0, LengthUnit.INCHES);

            Assert.AreEqual(2.0, result.Value, 0.0001);
        }

        // Reverse order
        [TestMethod]
        public void Add_InchPlusFeet_ReturnsInches()
        {
            var result = QuantityMeasurementService.AddLengths(
                12.0, LengthUnit.INCHES,
                1.0, LengthUnit.FEET);

            Assert.AreEqual(24.0, result.Value, 0.0001);
        }

        // Yard + Feet
        [TestMethod]
        public void Add_YardPlusFeet_ReturnsYards()
        {
            var result = QuantityMeasurementService.AddLengths(
                1.0, LengthUnit.YARDS,
                3.0, LengthUnit.FEET);

            Assert.AreEqual(2.0, result.Value, 0.0001);
        }

        // With zero
        [TestMethod]
        public void Add_WithZero_ReturnsSameValue()
        {
            var result = QuantityMeasurementService.AddLengths(
                5.0, LengthUnit.FEET,
                0.0, LengthUnit.INCHES);

            Assert.AreEqual(5.0, result.Value, 0.0001);
        }

        // Negative values
        [TestMethod]
        public void Add_NegativeValues_ReturnsCorrect()
        {
            var result = QuantityMeasurementService.AddLengths(
                5.0, LengthUnit.FEET,
                -2.0, LengthUnit.FEET);

            Assert.AreEqual(3.0, result.Value, 0.0001);
        }
    }
}