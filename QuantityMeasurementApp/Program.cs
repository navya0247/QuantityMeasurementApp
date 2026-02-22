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
                Console.WriteLine("\n--- Quantity Measurement Menu ---");
                Console.WriteLine("1. Check Feet Equality");
                Console.WriteLine("2. Check Inches Equality");
                Console.WriteLine("3. Check Length Equality (Generic)");
                Console.WriteLine("4. Exit");
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

                            Console.WriteLine("\nSelect Unit:");
                            Console.WriteLine("1 = Feet");
                            Console.WriteLine("2 = Inches");
                            Console.WriteLine("3 = Yards");
                            Console.WriteLine("4 = Centimeters");
                            LengthUnit u1 = (LengthUnit)(Convert.ToInt32(Console.ReadLine()) - 1);

                            Console.Write("\nEnter second value: ");
                            double v2 = Convert.ToDouble(Console.ReadLine());

                            Console.WriteLine("\nSelect Unit:");
                            Console.WriteLine("1 = Feet");
                            Console.WriteLine("2 = Inches");
                            Console.WriteLine("3 = Yards");
                            Console.WriteLine("4 = Centimeters");
                            LengthUnit u2 = (LengthUnit)(Convert.ToInt32(Console.ReadLine()) - 1);

                            Console.WriteLine("\nEqual: " +
                                QuantityMeasurementService.AreLengthEqual(v1, u1, v2, u2));
                            break;

                        case 4:
                            Console.WriteLine("Exiting...");
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

            } while (choice != 4);
        }
    }
}