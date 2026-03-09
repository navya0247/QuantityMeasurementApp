using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Tests
{
    /// <summary>
    /// UC11
    /// Test cases for Volume Measurement using Generic Quantity class.
    /// </summary>
    [TestClass]
    public class VolumeMeasurementTests
    {
        /// <summary>
        /// Test equality of litre to litre.
        /// </summary>
        [TestMethod]
        public void LitreToLitre_Equality()
        {
            var v1 = new Quantity<VolumeUnit>(1, VolumeUnit.LITRE);
            var v2 = new Quantity<VolumeUnit>(1, VolumeUnit.LITRE);

            Assert.IsTrue(v1.Equals(v2));
        }

        /// <summary>
        /// Test equality of litre to millilitre.
        /// </summary>
        [TestMethod]
        public void LitreToMillilitre_Equality()
        {
            var v1 = new Quantity<VolumeUnit>(1, VolumeUnit.LITRE);
            var v2 = new Quantity<VolumeUnit>(1000, VolumeUnit.MILLILITRE);

            Assert.IsTrue(v1.Equals(v2));
        }

        /// <summary>
        /// Test gallon to litre equality.
        /// </summary>
        [TestMethod]
        public void GallonToLitre_Equality()
        {
            var v1 = new Quantity<VolumeUnit>(1, VolumeUnit.GALLON);
            var v2 = new Quantity<VolumeUnit>(3.78541, VolumeUnit.LITRE);

            Assert.IsTrue(v1.Equals(v2));
        }

        /// <summary>
        /// Test conversion from litre to millilitre.
        /// </summary>
        [TestMethod]
        public void Convert_LitreToMillilitre()
        {
            var v = new Quantity<VolumeUnit>(1, VolumeUnit.LITRE);

            var result = v.ConvertTo(VolumeUnit.MILLILITRE);

            Assert.AreEqual(1000, result.Value, 0.001);
        }

        /// <summary>
        /// Test conversion from gallon to litre.
        /// </summary>
        [TestMethod]
        public void Convert_GallonToLitre()
        {
            var v = new Quantity<VolumeUnit>(1, VolumeUnit.GALLON);

            var result = v.ConvertTo(VolumeUnit.LITRE);

            Assert.AreEqual(3.78541, result.Value, 0.001);
        }

        /// <summary>
        /// Test addition of litre and millilitre.
        /// </summary>
        [TestMethod]
        public void Add_LitrePlusMillilitre()
        {
            var v1 = new Quantity<VolumeUnit>(1, VolumeUnit.LITRE);
            var v2 = new Quantity<VolumeUnit>(1000, VolumeUnit.MILLILITRE);

            var result = v1.Add(v2, VolumeUnit.LITRE);

            Assert.AreEqual(2, result.Value, 0.001);
        }

        /// <summary>
        /// Test addition of gallon and litre.
        /// </summary>
        [TestMethod]
        public void Add_GallonPlusLitre()
        {
            var v1 = new Quantity<VolumeUnit>(1, VolumeUnit.GALLON);
            var v2 = new Quantity<VolumeUnit>(3.78541, VolumeUnit.LITRE);

            var result = v1.Add(v2, VolumeUnit.GALLON);

            Assert.AreEqual(2, result.Value, 0.01);
        }
    }
}