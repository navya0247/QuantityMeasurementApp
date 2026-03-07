using System;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Services;

namespace QuantityMeasurementApp
{
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
                Console.WriteLine("8. Exit");

                Console.Write("Enter choice: ");

                choice = Convert.ToInt32(Console.ReadLine());

                try
                {
                    switch (choice)
                    {
                        // ---------- UC1 ----------
                        case 1:
                            Console.Write("Enter first feet value: ");
                            double f1 = Convert.ToDouble(Console.ReadLine());

                            Console.Write("Enter second feet value: ");
                            double f2 = Convert.ToDouble(Console.ReadLine());

                            Console.WriteLine("Equal: " +
                                QuantityMeasurementService.AreFeetEqual(f1, f2));
                            break;

                        // ---------- UC2 ----------
                        case 2:
                            Console.Write("Enter first inches value: ");
                            double i1 = Convert.ToDouble(Console.ReadLine());

                            Console.Write("Enter second inches value: ");
                            double i2 = Convert.ToDouble(Console.ReadLine());

                            Console.WriteLine("Equal: " +
                                QuantityMeasurementService.AreInchesEqual(i1, i2));
                            break;

                        // ---------- UC3 + UC4 ----------
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

                            Console.WriteLine("Equal: " +
                                QuantityMeasurementService.AreLengthEqual(v1, u1, v2, u2));
                            break;

                        // ---------- UC5 ----------
                        case 4:
                            Console.Write("Enter value to convert: ");
                            double value = Convert.ToDouble(Console.ReadLine());

                            Console.WriteLine("From Unit:");
                            DisplayUnits();
                            LengthUnit from = (LengthUnit)(Convert.ToInt32(Console.ReadLine()) - 1);

                            Console.WriteLine("To Unit:");
                            DisplayUnits();
                            LengthUnit to = (LengthUnit)(Convert.ToInt32(Console.ReadLine()) - 1);

                            double result = QuantityMeasurementService.ConvertLength(value, from, to);

                            Console.WriteLine("Converted Value: " + result);
                            break;

                        // ---------- UC6 ----------
                        case 5:
                            Console.Write("Enter first value: ");
                            double a1 = Convert.ToDouble(Console.ReadLine());

                            Console.WriteLine("First Value Unit:");
                            DisplayUnits();
                            LengthUnit ua1 = (LengthUnit)(Convert.ToInt32(Console.ReadLine()) - 1);

                            Console.Write("Enter second value: ");
                            double a2 = Convert.ToDouble(Console.ReadLine());

                            Console.WriteLine("Second Value Unit:");
                            DisplayUnits();
                            LengthUnit ua2 = (LengthUnit)(Convert.ToInt32(Console.ReadLine()) - 1);

                            var sum = QuantityMeasurementService.AddLengths(a1, ua1, a2, ua2);

                            Console.WriteLine($"Result: {sum.Value} {sum.Unit}");
                            break;


                        // ---------- UC7 ----------
                        case 6:
                            Console.Write("Enter first value: ");
                            double b1 = Convert.ToDouble(Console.ReadLine());

                            Console.WriteLine("First Value Unit:");
                            DisplayUnits();
                            LengthUnit ub1 = (LengthUnit)(Convert.ToInt32(Console.ReadLine()) - 1);

                            Console.Write("Enter second value: ");
                            double b2 = Convert.ToDouble(Console.ReadLine());

                            Console.WriteLine("Second Value Unit:");
                            DisplayUnits();
                            LengthUnit ub2 = (LengthUnit)(Convert.ToInt32(Console.ReadLine()) - 1);

                            Console.WriteLine("Target Unit:");
                            DisplayUnits();
                            LengthUnit target = (LengthUnit)(Convert.ToInt32(Console.ReadLine()) - 1);

                            var resultUC7 = QuantityMeasurementService.AddLengths(
                                b1, ub1, b2, ub2, target);

                            Console.WriteLine($"Result: {resultUC7.Value} {resultUC7.Unit}");
                            break;


                        // ---------- UC8 ----------
                        case 7:

                            Console.WriteLine("\nUC8: Standalone LengthUnit Conversion Operations");

                            Console.Write("Enter value: ");
                            double inputValue = Convert.ToDouble(Console.ReadLine());

                            Console.WriteLine("Select Unit:");
                            DisplayUnits();
                            LengthUnit selectedUnit = (LengthUnit)(Convert.ToInt32(Console.ReadLine()) - 1);

                            // Convert to base unit (FEET)
                            double baseValue = selectedUnit.ConvertToBaseUnit(inputValue);

                            Console.WriteLine($"Converted to base unit (FEET): {baseValue}");

                            // Convert back from base unit
                            double reconverted = selectedUnit.ConvertFromBaseUnit(baseValue);

                            Console.WriteLine($"Converted back to {selectedUnit}: {reconverted}");

                            Console.WriteLine("UC8 Demonstration Complete: LengthUnit handles unit conversion.");

                            break;

                        case 8:
                            Console.WriteLine("Exiting application...");
                            break;

                        default:
                            Console.WriteLine("Invalid choice!");
                            break;
                    }
                }
                catch
                {
                    Console.WriteLine("Invalid input!");
                }

            } while (choice != 8);
        }

        // Helper method to show units menu
        private static void DisplayUnits()
        {
            Console.WriteLine("1 = FEET");
            Console.WriteLine("2 = INCHES");
            Console.WriteLine("3 = YARDS");
            Console.WriteLine("4 = CENTIMETERS");
        }
    }

}
