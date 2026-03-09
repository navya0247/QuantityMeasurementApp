using System;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp
{
    /// <summary>
    /// UC11     : Volume measurement operations
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            int choice;

            do
            {
                Console.WriteLine("\n========= Quantity Measurement Menu =========");
                Console.WriteLine("1. Check Feet Equality (UC1)");
                Console.WriteLine("2. Check Inches Equality (UC2)");
                Console.WriteLine("3. Check Generic Length Equality (UC3/UC4)");
                Console.WriteLine("4. Convert Length Units (UC5)");
                Console.WriteLine("5. Add Two Lengths (UC6)");
                Console.WriteLine("6. Add Two Lengths With Target Unit (UC7)");
                Console.WriteLine("7. Standalone LengthUnit Conversion Operations (UC8)");
                Console.WriteLine("8. Weight Measurement Operations (UC9)");
                Console.WriteLine("9. Generic Quantity Demo (UC10)");
                Console.WriteLine("10. Volume Measurement Operations (UC11)");
                Console.WriteLine("11. Subtraction and Division Operations (UC12)");
                Console.WriteLine("12. Exit");

                Console.Write("Enter choice: ");
                choice = Convert.ToInt32(Console.ReadLine());

                try
                {
                    switch (choice)
                    {

                        // ---------------- UC1 ----------------
                        case 1:

                            Console.Write("Enter first feet value: ");
                            double f1 = Convert.ToDouble(Console.ReadLine());

                            Console.Write("Enter second feet value: ");
                            double f2 = Convert.ToDouble(Console.ReadLine());

                            var feet1 = new Quantity<LengthUnit>(f1, LengthUnit.FEET);
                            var feet2 = new Quantity<LengthUnit>(f2, LengthUnit.FEET);

                            Console.WriteLine("Equal: " + feet1.Equals(feet2));

                            break;

                        // ---------------- UC2 ----------------
                        case 2:

                            Console.Write("Enter first inches value: ");
                            double i1 = Convert.ToDouble(Console.ReadLine());

                            Console.Write("Enter second inches value: ");
                            double i2 = Convert.ToDouble(Console.ReadLine());

                            var inch1 = new Quantity<LengthUnit>(i1, LengthUnit.INCHES);
                            var inch2 = new Quantity<LengthUnit>(i2, LengthUnit.INCHES);

                            Console.WriteLine("Equal: " + inch1.Equals(inch2));

                            break;

                        // ---------------- UC3 / UC4 ----------------
                        case 3:

                            Console.Write("Enter first value: ");
                            double v1 = Convert.ToDouble(Console.ReadLine());

                            Console.WriteLine("Select Unit:");
                            DisplayUnits();
                            LengthUnit u1 = (LengthUnit)(Convert.ToInt32(Console.ReadLine()) - 1);

                            Console.Write("Enter second value: ");
                            double v2 = Convert.ToDouble(Console.ReadLine());

                            Console.WriteLine("Select Unit:");
                            DisplayUnits();
                            LengthUnit u2 = (LengthUnit)(Convert.ToInt32(Console.ReadLine()) - 1);

                            var q1 = new Quantity<LengthUnit>(v1, u1);
                            var q2 = new Quantity<LengthUnit>(v2, u2);

                            Console.WriteLine("Equal: " + q1.Equals(q2));

                            break;

                        // ---------------- UC5 ----------------
                        case 4:

                            Console.Write("Enter value to convert: ");
                            double value = Convert.ToDouble(Console.ReadLine());

                            Console.WriteLine("From Unit:");
                            DisplayUnits();
                            LengthUnit from = (LengthUnit)(Convert.ToInt32(Console.ReadLine()) - 1);

                            Console.WriteLine("To Unit:");
                            DisplayUnits();
                            LengthUnit to = (LengthUnit)(Convert.ToInt32(Console.ReadLine()) - 1);

                            var quantity = new Quantity<LengthUnit>(value, from);

                            Console.WriteLine("Converted Value: " +
                                quantity.ConvertTo(to));

                            break;

                        // ---------------- UC6 ----------------
                        case 5:

                            var add1 = new Quantity<LengthUnit>(1, LengthUnit.FEET);
                            var add2 = new Quantity<LengthUnit>(12, LengthUnit.INCHES);

                            var result = add1.Add(add2, LengthUnit.FEET);

                            Console.WriteLine($"Result: {result}");

                            break;

                        // ---------------- UC7 ----------------
                        case 6:

                            var a = new Quantity<LengthUnit>(1, LengthUnit.FEET);
                            var b = new Quantity<LengthUnit>(24, LengthUnit.INCHES);

                            var resultUC7 = a.Add(b, LengthUnit.INCHES);

                            Console.WriteLine($"Result: {resultUC7}");

                            break;

                        // ---------------- UC8 ----------------
                        case 7:

                            Console.Write("Enter value: ");
                            double inputValue = Convert.ToDouble(Console.ReadLine());

                            Console.WriteLine("Select Unit:");
                            DisplayUnits();
                            LengthUnit selectedUnit = (LengthUnit)(Convert.ToInt32(Console.ReadLine()) - 1);

                            double baseValue = selectedUnit.ConvertToBaseUnit(inputValue);

                            Console.WriteLine($"Converted to FEET: {baseValue}");

                            break;

                        // ---------------- UC9 ----------------
                        case 8:

                            var weight1 = new Quantity<WeightUnit>(1, WeightUnit.KILOGRAM);
                            var weight2 = new Quantity<WeightUnit>(1000, WeightUnit.GRAM);

                            var weightSum = weight1.Add(weight2, WeightUnit.KILOGRAM);

                            Console.WriteLine($"Result: {weightSum}");

                            break;

                        // ---------------- UC10 ----------------
                        case 9:

                            Console.WriteLine("UC10 Generic Quantity Demo");

                            var length = new Quantity<LengthUnit>(1, LengthUnit.FEET);
                            var lengthConverted = length.ConvertTo(LengthUnit.INCHES);

                            Console.WriteLine($"1 FEET = {lengthConverted}");

                            var weight = new Quantity<WeightUnit>(1, WeightUnit.KILOGRAM);
                            var weightConverted = weight.ConvertTo(WeightUnit.GRAM);

                            Console.WriteLine($"1 KG = {weightConverted}");

                            break;

                        // ---------------- UC11 ----------------
                        case 10:

                            Console.WriteLine("UC11 Volume Measurement Demo");

                            var volume1 = new Quantity<VolumeUnit>(1, VolumeUnit.LITRE);
                            var volume2 = new Quantity<VolumeUnit>(1000, VolumeUnit.MILLILITRE);

                            Console.WriteLine($"1 L == 1000 mL → {volume1.Equals(volume2)}");

                            var converted = volume1.ConvertTo(VolumeUnit.MILLILITRE);
                            Console.WriteLine($"1 L in mL → {converted}");

                            var sum = volume1.Add(volume2, VolumeUnit.LITRE);
                            Console.WriteLine($"1 L + 1000 mL → {sum}");

                            break;

                        // ---------------- UC12 ----------------

                        case 11:
                            {
                                Console.WriteLine("UC12 Subtraction and Division Demo");

                                var r1 = new Quantity<LengthUnit>(10, LengthUnit.FEET);
                                var r2 = new Quantity<LengthUnit>(6, LengthUnit.INCHES);

                                var subtractResult = r1.Subtract(r2);

                                Console.WriteLine($"10 FEET - 6 INCHES = {subtractResult}");

                                var r3 = new Quantity<LengthUnit>(2, LengthUnit.FEET);

                                var divideResult = r1.Divide(r3);

                                Console.WriteLine($"10 FEET ÷ 2 FEET = {divideResult}");

                                break;
                            }

                        case 12:

                            Console.WriteLine("Exiting application...");
                            break;
                    }
                }
                catch
                {
                    Console.WriteLine("Invalid input!");
                }

            } while (choice != 12);
        }

        /// <summary>
        /// Displays length units.
        /// </summary>
        private static void DisplayUnits()
        {
            Console.WriteLine("1 = FEET");
            Console.WriteLine("2 = INCHES");
            Console.WriteLine("3 = YARDS");
            Console.WriteLine("4 = CENTIMETERS");
        }

        /// <summary>
        /// Displays weight units.
        /// </summary>
        private static void DisplayWeightUnits()
        {
            Console.WriteLine("1 = KILOGRAM");
            Console.WriteLine("2 = GRAM");
            Console.WriteLine("3 = POUND");
        }
    }
}