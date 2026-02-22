using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Services;

namespace QuantityMeasurementApp.Tests
{
    [TestClass]
    public class QuantityMeasurementTests
    {
        //  FEET TEST CASES 

        // Test 1: Same value should return true
        [TestMethod]
        public void FeetEquality_SameValue_ReturnsTrue()
        {
            bool result = QuantityMeasurementService.AreFeetEqual(1.0, 1.0);
            Assert.IsTrue(result);
        }

        // Test 2: Different values should return false
        [TestMethod]
        public void FeetEquality_DifferentValue_ReturnsFalse()
        {
            bool result = QuantityMeasurementService.AreFeetEqual(1.0, 2.0);
            Assert.IsFalse(result);
        }

        // Test 3: Same reference object should return true
        [TestMethod]
        public void FeetEquality_SameReference_ReturnsTrue()
        {
            Feet f1 = new Feet(1.0);
            Assert.IsTrue(f1.Equals(f1));
        }

        // Test 4: Null comparison should return false
        [TestMethod]
        public void FeetEquality_NullComparison_ReturnsFalse()
        {
            Feet f1 = new Feet(1.0);
            Assert.IsFalse(f1.Equals(null));
        }

        // Test 5: Non-numeric / invalid input simulation (null object)
        [TestMethod]
        public void FeetEquality_InvalidInput_ReturnsFalse()
        {
            Feet f1 = new Feet(1.0);
            Feet f2 = null;

            Assert.IsFalse(f1.Equals(f2));
        }


        //  INCHES TEST CASES 

        // Test 6: Same inches value should return true
        [TestMethod]
        public void InchesEquality_SameValue_ReturnsTrue()
        {
            bool result = QuantityMeasurementService.AreInchesEqual(5.0, 5.0);
            Assert.IsTrue(result);
        }

        // Test 7: Different inches values should return false
        [TestMethod]
        public void InchesEquality_DifferentValue_ReturnsFalse()
        {
            bool result = QuantityMeasurementService.AreInchesEqual(5.0, 6.0);
            Assert.IsFalse(result);
        }

        // Test 8: Same reference object should return true
        [TestMethod]
        public void InchesEquality_SameReference_ReturnsTrue()
        {
            Inches i1 = new Inches(2.0);
            Assert.IsTrue(i1.Equals(i1));
        }

        // Test 9: Null comparison should return false
        [TestMethod]
        public void InchesEquality_NullComparison_ReturnsFalse()
        {
            Inches i1 = new Inches(2.0);
            Assert.IsFalse(i1.Equals(null));
        }

        // Test 10: Invalid input simulation (null object)
        [TestMethod]
        public void InchesEquality_InvalidInput_ReturnsFalse()
        {
            Inches i1 = new Inches(2.0);
            Inches i2 = null;

            Assert.IsFalse(i1.Equals(i2));
        }
    }
}