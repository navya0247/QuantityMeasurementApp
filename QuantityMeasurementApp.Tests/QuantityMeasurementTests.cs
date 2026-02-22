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
            Assert.IsTrue(QuantityMeasurementService.AreLengthEqual(5.0, LengthUnit.INCH, 5.0, LengthUnit.INCH));
        }

        [TestMethod]
        public void FeetToInch_Equivalent_ReturnsTrue()
        {
            Assert.IsTrue(QuantityMeasurementService.AreLengthEqual(1.0, LengthUnit.FEET, 12.0, LengthUnit.INCH));
        }

        [TestMethod]
        public void InchToFeet_Equivalent_ReturnsTrue()
        {
            Assert.IsTrue(QuantityMeasurementService.AreLengthEqual(12.0, LengthUnit.INCH, 1.0, LengthUnit.FEET));
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
    }
}