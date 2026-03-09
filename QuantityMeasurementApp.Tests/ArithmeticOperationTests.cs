using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Tests
{
    /// <summary>
    /// UC12
    /// Test cases for subtraction and division operations.
    /// </summary>
    [TestClass]
    public class ArithmeticOperationTests
    {
        [TestMethod]
        public void Subtract_SameUnit_Length()
        {
            var q1 = new Quantity<LengthUnit>(10, LengthUnit.FEET);
            var q2 = new Quantity<LengthUnit>(5, LengthUnit.FEET);

            var result = q1.Subtract(q2);

            Assert.AreEqual(5, result.Value, 0.001);
        }

        [TestMethod]
        public void Subtract_CrossUnit_Length()
        {
            var q1 = new Quantity<LengthUnit>(10, LengthUnit.FEET);
            var q2 = new Quantity<LengthUnit>(6, LengthUnit.INCHES);

            var result = q1.Subtract(q2);

            Assert.AreEqual(9.5, result.Value, 0.1);
        }

        [TestMethod]
        public void Subtract_LitreMinusMillilitre()
        {
            var q1 = new Quantity<VolumeUnit>(5, VolumeUnit.LITRE);
            var q2 = new Quantity<VolumeUnit>(500, VolumeUnit.MILLILITRE);

            var result = q1.Subtract(q2);

            Assert.AreEqual(4.5, result.Value, 0.001);
        }

        [TestMethod]
        public void Divide_Lengths()
        {
            var q1 = new Quantity<LengthUnit>(10, LengthUnit.FEET);
            var q2 = new Quantity<LengthUnit>(2, LengthUnit.FEET);

            var result = q1.Divide(q2);

            Assert.AreEqual(5, result, 0.001);
        }

        [TestMethod]
        public void Divide_Litre()
        {
            var q1 = new Quantity<VolumeUnit>(10, VolumeUnit.LITRE);
            var q2 = new Quantity<VolumeUnit>(5, VolumeUnit.LITRE);

            var result = q1.Divide(q2);

            Assert.AreEqual(2, result, 0.001);
        }

        [TestMethod]
        public void Divide_ByZero()
        {
            var q1 = new Quantity<LengthUnit>(10, LengthUnit.FEET);
            var q2 = new Quantity<LengthUnit>(0, LengthUnit.FEET);

            try
            {
                q1.Divide(q2);
                Assert.Fail("Expected ArithmeticException was not thrown.");
            }
            catch (ArithmeticException)
            {
                Assert.IsTrue(true);
            }
        }
    }
}