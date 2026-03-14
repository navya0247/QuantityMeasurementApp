using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.ModelLayer.Models;
using QuantityMeasurementApp.BusinessLayer.Services;

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
            var q1 = new Quantity<LengthEnum>(10, LengthEnum.FEET);
            var q2 = new Quantity<LengthEnum>(5, LengthEnum.FEET);

            var result = q1.Subtract(q2);

            Assert.AreEqual(5, result.Value, 0.001);
        }

        [TestMethod]
        public void Subtract_CrossUnit_Length()
        {
            var q1 = new Quantity<LengthEnum>(10, LengthEnum.FEET);
            var q2 = new Quantity<LengthEnum>(6, LengthEnum.INCHES);

            var result = q1.Subtract(q2);

            Assert.AreEqual(9.5, result.Value, 0.1);
        }

        [TestMethod]
        public void Subtract_LitreMinusMillilitre()
        {
            var q1 = new Quantity<VolumeEnum>(5, VolumeEnum.LITRE);
            var q2 = new Quantity<VolumeEnum>(500, VolumeEnum.MILLILITRE);

            var result = q1.Subtract(q2);

            Assert.AreEqual(4.5, result.Value, 0.001);
        }

        [TestMethod]
        public void Divide_Lengths()
        {
            var q1 = new Quantity<LengthEnum>(10, LengthEnum.FEET);
            var q2 = new Quantity<LengthEnum>(2, LengthEnum.FEET);

            var result = q1.Divide(q2);

            Assert.AreEqual(5, result, 0.001);
        }

        [TestMethod]
        public void Divide_Litre()
        {
            var q1 = new Quantity<VolumeEnum>(10, VolumeEnum.LITRE);
            var q2 = new Quantity<VolumeEnum>(5, VolumeEnum.LITRE);

            var result = q1.Divide(q2);

            Assert.AreEqual(2, result, 0.001);
        }

        [TestMethod]
        public void Divide_ByZero()
        {
            var q1 = new Quantity<LengthEnum>(10, LengthEnum.FEET);
            var q2 = new Quantity<LengthEnum>(0, LengthEnum.FEET);

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