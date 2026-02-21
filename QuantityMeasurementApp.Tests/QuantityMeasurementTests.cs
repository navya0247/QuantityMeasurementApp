using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Services;

namespace QuantityMeasurementApp.Tests
{
    [TestClass]
    public class QuantityMeasurementTests
    {
        private QuantityMeasurementService service;

        // Runs before every test
        [TestInitialize]
        public void Setup()
        {
            service = new QuantityMeasurementService();
        }

        //  Same value test
        [TestMethod]
        public void testEquality_SameValue()
        {
            Feet f1 = new Feet(1.0);
            Feet f2 = new Feet(1.0);

            bool result = service.AreFeetEqual(f1, f2);

            Assert.IsTrue(result);
        }

        //  Different value test
        [TestMethod]
        public void testEquality_DifferentValue()
        {
            Feet f1 = new Feet(1.0);
            Feet f2 = new Feet(2.0);

            bool result = service.AreFeetEqual(f1, f2);

            Assert.IsFalse(result);
        }

        //  Null comparison test
        [TestMethod]
        public void testEquality_NullComparison()
        {
            Feet f1 = new Feet(1.0);

            bool result = service.AreFeetEqual(f1, null);

            Assert.IsFalse(result);
        }

        //  Same reference test (reflexive property)
        [TestMethod]
        public void testEquality_SameReference()
        {
            Feet f1 = new Feet(1.0);

            bool result = service.AreFeetEqual(f1, f1);

            Assert.IsTrue(result);
        }

        //  Non-numeric input scenario test
        [TestMethod]
        public void testEquality_NonNumericInput()
        {
            Feet f1 = new Feet(1.0);

            // Simulate invalid object (not created)
            Feet f2 = null;

            bool result = service.AreFeetEqual(f1, f2);

            Assert.IsFalse(result);
        }
    }
}