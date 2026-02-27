using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Services
{
    public class QuantityMeasurementService
    {
        // ---------- UC1 ----------
        // Method to check equality of two feet values
        public static bool AreFeetEqual(double v1, double v2)
        {
            Feet f1 = new Feet(v1);
            Feet f2 = new Feet(v2);

            return f1.Equals(f2);
        }

        // ---------- UC2 ----------
        // Method to check equality of two inches values
        public static bool AreInchesEqual(double v1, double v2)
        {
            Inches i1 = new Inches(v1);
            Inches i2 = new Inches(v2);

            return i1.Equals(i2);
        }

        // ---------- UC3 & UC4 ----------
        // Generic equality check for any length unit
        public static bool AreLengthEqual(double v1, LengthUnit u1, double v2, LengthUnit u2)
        {
            QuantityLength q1 = new QuantityLength(v1, u1);
            QuantityLength q2 = new QuantityLength(v2, u2);

            return q1.Equals(q2);
        }

        // ---------- UC5 ----------
        // Convert value from one unit to another
        public static double ConvertLength(double value, LengthUnit from, LengthUnit to)
        {
            return QuantityLength.Convert(value, from, to);
        }

        // UC6 ADDITION SERVICE METHOD
        public static QuantityLength AddLengths(double v1, LengthUnit u1, double v2, LengthUnit u2)
        {
            QuantityLength q1 = new QuantityLength(v1, u1);
            QuantityLength q2 = new QuantityLength(v2, u2);

            return QuantityLength.Add(q1, q2);
        }

        // ---------- UC7 ----------
        // Add Two Lengths With Explicit Target Unit
        public static QuantityLength AddLengths(
            double v1, LengthUnit u1,
            double v2, LengthUnit u2,
            LengthUnit targetUnit)
        {
            QuantityLength q1 = new QuantityLength(v1, u1);
            QuantityLength q2 = new QuantityLength(v2, u2);

            return QuantityLength.Add(q1, q2, targetUnit);
        }
    }
}