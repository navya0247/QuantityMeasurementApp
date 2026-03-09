using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Tests
{
    /// <summary>
    /// UC9 + UC10 Weight Tests using Generic Quantity<U>
    /// </summary>
    [TestClass]
    public class WeightMeasurementTests
    {
        // ---------- UC9 ----------
        [TestMethod]
        public void WeightEquality_KgToKg()
        {
            var w1 = new Quantity<WeightUnit>(1.0, WeightUnit.KILOGRAM);
            var w2 = new Quantity<WeightUnit>(1.0, WeightUnit.KILOGRAM);

            Assert.IsTrue(w1.Equals(w2));
        }

        [TestMethod]
        public void WeightEquality_KgToGram()
        {
            var w1 = new Quantity<WeightUnit>(1.0, WeightUnit.KILOGRAM);
            var w2 = new Quantity<WeightUnit>(1000.0, WeightUnit.GRAM);

            Assert.IsTrue(w1.Equals(w2));
        }

        [TestMethod]
        public void WeightEquality_KgToPound()
        {
            var w1 = new Quantity<WeightUnit>(1.0, WeightUnit.KILOGRAM);
            var w2 = new Quantity<WeightUnit>(2.20462, WeightUnit.POUND);

            Assert.IsTrue(w1.Equals(w2));
        }

        // ---------- Conversion ----------
        [TestMethod]
        public void WeightConversion_KgToGram()
        {
            var w = new Quantity<WeightUnit>(1.0, WeightUnit.KILOGRAM);

            var result = w.ConvertTo(WeightUnit.GRAM);

            Assert.AreEqual(1000.0, result.Value, 0.0001);
        }

        [TestMethod]
        public void WeightConversion_GramToKg()
        {
            var w = new Quantity<WeightUnit>(1000.0, WeightUnit.GRAM);

            var result = w.ConvertTo(WeightUnit.KILOGRAM);

            Assert.AreEqual(1.0, result.Value, 0.0001);
        }

        // ---------- Addition ----------
        [TestMethod]
        public void WeightAddition_KgPlusKg()
        {
            var w1 = new Quantity<WeightUnit>(1.0, WeightUnit.KILOGRAM);
            var w2 = new Quantity<WeightUnit>(2.0, WeightUnit.KILOGRAM);

            var result = w1.Add(w2, WeightUnit.KILOGRAM);

            Assert.AreEqual(3.0, result.Value, 0.0001);
        }

        [TestMethod]
        public void WeightAddition_KgPlusGram()
        {
            var w1 = new Quantity<WeightUnit>(1.0, WeightUnit.KILOGRAM);
            var w2 = new Quantity<WeightUnit>(1000.0, WeightUnit.GRAM);

            var result = w1.Add(w2, WeightUnit.KILOGRAM);

            Assert.AreEqual(2.0, result.Value, 0.0001);
        }

        // ---------- Edge cases ----------
        [TestMethod]
        public void WeightEquality_Zero()
        {
            var w1 = new Quantity<WeightUnit>(0.0, WeightUnit.KILOGRAM);
            var w2 = new Quantity<WeightUnit>(0.0, WeightUnit.GRAM);

            Assert.IsTrue(w1.Equals(w2));
        }

        [TestMethod]
        public void WeightEquality_Negative()
        {
            var w1 = new Quantity<WeightUnit>(-1.0, WeightUnit.KILOGRAM);
            var w2 = new Quantity<WeightUnit>(-1000.0, WeightUnit.GRAM);

            Assert.IsTrue(w1.Equals(w2));
        }
    }
}