using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Tests
{
    [TestClass]
    public class WeightMeasurementTests
    {
        //  UC9 – WEIGHT EQUALITY 

        // Same unit equality
        [TestMethod]
        public void WeightEquality_KgToKg_SameValue()
        {
            QuantityWeight w1 = new QuantityWeight(1.0, WeightUnit.KILOGRAM);
            QuantityWeight w2 = new QuantityWeight(1.0, WeightUnit.KILOGRAM);

            Assert.IsTrue(w1.Equals(w2));
        }

        // Kilogram to gram equality
        [TestMethod]
        public void WeightEquality_KgToGram()
        {
            QuantityWeight w1 = new QuantityWeight(1.0, WeightUnit.KILOGRAM);
            QuantityWeight w2 = new QuantityWeight(1000.0, WeightUnit.GRAM);

            Assert.IsTrue(w1.Equals(w2));
        }

        // Kilogram to pound equality
        [TestMethod]
        public void WeightEquality_KgToPound()
        {
            QuantityWeight w1 = new QuantityWeight(1.0, WeightUnit.KILOGRAM);
            QuantityWeight w2 = new QuantityWeight(2.20462, WeightUnit.POUND);

            Assert.IsTrue(w1.Equals(w2));
        }

        //  WEIGHT CONVERSION 

        // KG - Gram
        [TestMethod]
        public void WeightConversion_KgToGram()
        {
            QuantityWeight w = new QuantityWeight(1.0, WeightUnit.KILOGRAM);

            var result = w.ConvertTo(WeightUnit.GRAM);

            Assert.AreEqual(1000.0, result.Value, 0.0001);
        }

        // Gram - KG
        [TestMethod]
        public void WeightConversion_GramToKg()
        {
            QuantityWeight w = new QuantityWeight(1000.0, WeightUnit.GRAM);

            var result = w.ConvertTo(WeightUnit.KILOGRAM);

            Assert.AreEqual(1.0, result.Value, 0.0001);
        }

        // Pound - KG
        [TestMethod]
        public void WeightConversion_PoundToKg()
        {
            QuantityWeight w = new QuantityWeight(2.20462, WeightUnit.POUND);

            var result = w.ConvertTo(WeightUnit.KILOGRAM);

            Assert.AreEqual(1.0, result.Value, 0.01);
        }

        //  WEIGHT ADDITION 

        // Same unit addition
        [TestMethod]
        public void WeightAddition_KgPlusKg()
        {
            var result = QuantityWeight.Add(
                new QuantityWeight(1.0, WeightUnit.KILOGRAM),
                new QuantityWeight(2.0, WeightUnit.KILOGRAM));

            Assert.AreEqual(3.0, result.Value, 0.0001);
        }

        // Cross unit addition
        [TestMethod]
        public void WeightAddition_KgPlusGram()
        {
            var result = QuantityWeight.Add(
                new QuantityWeight(1.0, WeightUnit.KILOGRAM),
                new QuantityWeight(1000.0, WeightUnit.GRAM));

            Assert.AreEqual(2.0, result.Value, 0.0001);
        }

        // Explicit target unit
        [TestMethod]
        public void WeightAddition_TargetUnit_Gram()
        {
            var result = QuantityWeight.Add(
                new QuantityWeight(1.0, WeightUnit.KILOGRAM),
                new QuantityWeight(1000.0, WeightUnit.GRAM),
                WeightUnit.GRAM);

            Assert.AreEqual(2000.0, result.Value, 0.0001);
        }

        //  EDGE CASES 

        // Zero value
        [TestMethod]
        public void WeightEquality_Zero()
        {
            QuantityWeight w1 = new QuantityWeight(0.0, WeightUnit.KILOGRAM);
            QuantityWeight w2 = new QuantityWeight(0.0, WeightUnit.GRAM);

            Assert.IsTrue(w1.Equals(w2));
        }

        // Negative value
        [TestMethod]
        public void WeightEquality_Negative()
        {
            QuantityWeight w1 = new QuantityWeight(-1.0, WeightUnit.KILOGRAM);
            QuantityWeight w2 = new QuantityWeight(-1000.0, WeightUnit.GRAM);

            Assert.IsTrue(w1.Equals(w2));
        }
    }
}