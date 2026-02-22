using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Services;

namespace QuantityMeasurementApp.Tests
{
    [TestClass]
    public class QuantityMeasurementTests
    {
        // UC1 TESTS – FEET


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



        // UC2 TESTS – INCHES


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


        // UC3 TESTS – GENERIC LENGTH


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

        [TestMethod]
        public void NullComparison_ReturnsFalse()
        {
            QuantityLength q = new QuantityLength(1.0, LengthUnit.FEET);
            Assert.IsFalse(q.Equals(null));
        }

        [TestMethod]
        public void SameReference_ReturnsTrue()
        {
            QuantityLength q = new QuantityLength(1.0, LengthUnit.FEET);
            Assert.IsTrue(q.Equals(q));
        }


        //  UC4 TESTS 

        // Yard same value
        [TestMethod]
        public void YardToYard_SameValue_ReturnsTrue()
        {
            Assert.IsTrue(
                QuantityMeasurementService.AreLengthEqual(
                    1.0, LengthUnit.YARDS,
                    1.0, LengthUnit.YARDS));
        }

        // Yard to Feet equivalent
        [TestMethod]
        public void YardToFeet_Equivalent_ReturnsTrue()
        {
            Assert.IsTrue(
                QuantityMeasurementService.AreLengthEqual(
                    1.0, LengthUnit.YARDS,
                    3.0, LengthUnit.FEET));
        }

        // Yard to Inches equivalent
        [TestMethod]
        public void YardToInches_Equivalent_ReturnsTrue()
        {
            Assert.IsTrue(
                QuantityMeasurementService.AreLengthEqual(
                    1.0, LengthUnit.YARDS,
                    36.0, LengthUnit.INCHES));
        }

        // Yard different value
        [TestMethod]
        public void YardDifferentValue_ReturnsFalse()
        {
            Assert.IsFalse(
                QuantityMeasurementService.AreLengthEqual(
                    1.0, LengthUnit.YARDS,
                    2.0, LengthUnit.YARDS));
        }

        // Centimeter same value
        [TestMethod]
        public void CmToCm_SameValue_ReturnsTrue()
        {
            Assert.IsTrue(
                QuantityMeasurementService.AreLengthEqual(
                    2.0, LengthUnit.CENTIMETERS,
                    2.0, LengthUnit.CENTIMETERS));
        }

        // Centimeter to Inch equivalent
        [TestMethod]
        public void CmToInch_Equivalent_ReturnsTrue()
        {
            Assert.IsTrue(
                QuantityMeasurementService.AreLengthEqual(
                    1.0, LengthUnit.CENTIMETERS,
                    0.393701, LengthUnit.INCHES));
        }

        // Centimeter to Feet not equal
        [TestMethod]
        public void CmToFeet_NotEqual_ReturnsFalse()
        {
            Assert.IsFalse(
                QuantityMeasurementService.AreLengthEqual(
                    1.0, LengthUnit.CENTIMETERS,
                    1.0, LengthUnit.FEET));
        }

        // Multi-unit transitive property
        [TestMethod]
        public void MultiUnit_TransitiveProperty_ReturnsTrue()
        {
            bool a = QuantityMeasurementService.AreLengthEqual(
                1.0, LengthUnit.YARDS,
                3.0, LengthUnit.FEET);

            bool b = QuantityMeasurementService.AreLengthEqual(
                3.0, LengthUnit.FEET,
                36.0, LengthUnit.INCHES);

            bool c = QuantityMeasurementService.AreLengthEqual(
                1.0, LengthUnit.YARDS,
                36.0, LengthUnit.INCHES);

            Assert.IsTrue(a && b && c);
        }
    }
}