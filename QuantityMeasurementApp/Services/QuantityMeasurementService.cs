using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Services
{
    /// <summary>
    /// UC10:
    /// Service class updated to use Generic Quantity<U> instead of
    /// QuantityLength and QuantityWeight classes.
    /// </summary>
    public class QuantityMeasurementService
    {
        // ---------- UC1 ----------
        /// <summary>
        /// Check equality of two feet values
        /// </summary>
        public static bool AreFeetEqual(double v1, double v2)
        {
            Feet f1 = new Feet(v1);
            Feet f2 = new Feet(v2);

            return f1.Equals(f2);
        }

        // ---------- UC2 ----------
        /// <summary>
        /// Check equality of two inches values
        /// </summary>
        public static bool AreInchesEqual(double v1, double v2)
        {
            Inches i1 = new Inches(v1);
            Inches i2 = new Inches(v2);

            return i1.Equals(i2);
        }

        // ---------- UC3 & UC4 ----------
        /// <summary>
        /// Generic equality check using Quantity<U>
        /// </summary>
        public static bool AreLengthEqual(double v1, LengthUnit u1, double v2, LengthUnit u2)
        {
            var q1 = new Quantity<LengthUnit>(v1, u1);
            var q2 = new Quantity<LengthUnit>(v2, u2);

            return q1.Equals(q2);
        }

        // ---------- UC5 ----------
        /// <summary>
        /// Convert length units using generic Quantity
        /// </summary>
        public static double ConvertLength(double value, LengthUnit from, LengthUnit to)
        {
            var quantity = new Quantity<LengthUnit>(value, from);

            return quantity.ConvertTo(to).Value;
        }

        // ---------- UC6 ----------
        /// <summary>
        /// Add two lengths
        /// </summary>
        public static Quantity<LengthUnit> AddLengths(
            double v1, LengthUnit u1,
            double v2, LengthUnit u2)
        {
            var q1 = new Quantity<LengthUnit>(v1, u1);
            var q2 = new Quantity<LengthUnit>(v2, u2);

            return q1.Add(q2, u1);
        }

        // ---------- UC7 ----------
        /// <summary>
        /// Add two lengths with target unit
        /// </summary>
        public static Quantity<LengthUnit> AddLengths(
            double v1, LengthUnit u1,
            double v2, LengthUnit u2,
            LengthUnit targetUnit)
        {
            var q1 = new Quantity<LengthUnit>(v1, u1);
            var q2 = new Quantity<LengthUnit>(v2, u2);

            return q1.Add(q2, targetUnit);
        }

        // ---------- UC9 ----------
        /// <summary>
        /// Weight equality using generic Quantity
        /// </summary>
        public static bool AreWeightEqual(double v1, WeightUnit u1, double v2, WeightUnit u2)
        {
            var w1 = new Quantity<WeightUnit>(v1, u1);
            var w2 = new Quantity<WeightUnit>(v2, u2);

            return w1.Equals(w2);
        }

        //------ UC10 --------
        /// <summary>
        /// Convert weight units
        /// </summary>
        public static Quantity<WeightUnit> ConvertWeight(double value, WeightUnit from, WeightUnit to)
        {
            var weight = new Quantity<WeightUnit>(value, from);

            return weight.ConvertTo(to);
        }

        /// <summary>
        /// Add two weights
        /// </summary>
        public static Quantity<WeightUnit> AddWeights(
            double v1, WeightUnit u1,
            double v2, WeightUnit u2)
        {
            var w1 = new Quantity<WeightUnit>(v1, u1);
            var w2 = new Quantity<WeightUnit>(v2, u2);

            return w1.Add(w2, u1);
        }



        /// <summary>
        /// UC11
        /// Checks equality between two volume quantities.
        /// </summary>
        public bool AreVolumesEqual(double v1, VolumeUnit u1, double v2, VolumeUnit u2)
        {
            var q1 = new Quantity<VolumeUnit>(v1, u1);
            var q2 = new Quantity<VolumeUnit>(v2, u2);

            return q1.Equals(q2);
        }

        /// <summary>
        /// UC11
        /// Converts volume from one unit to another.
        /// </summary>
        public double ConvertVolume(double value, VolumeUnit from, VolumeUnit to)
        {
            var quantity = new Quantity<VolumeUnit>(value, from);

            return quantity.ConvertTo(to).Value;
        }

        /// <summary>
        /// UC11
        /// Adds two volume quantities and returns result in target unit.
        /// </summary>
        public double AddVolumes(double v1, VolumeUnit u1, double v2, VolumeUnit u2, VolumeUnit target)
        {
            var q1 = new Quantity<VolumeUnit>(v1, u1);
            var q2 = new Quantity<VolumeUnit>(v2, u2);

            return q1.Add(q2, target).Value;
        }
    }
}