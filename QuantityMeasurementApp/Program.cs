using System;
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
                // Display menu options
                Console.WriteLine("\n--- Quantity Measurement Menu ---");
                Console.WriteLine("1. Check Feet Equality");
                Console.WriteLine("2. Check Inches Equality");
                Console.WriteLine("3. Exit");
                Console.Write("Enter your choice: ");

                choice = Convert.ToInt32(Console.ReadLine());

                try
                {
                    switch (choice)
                    {
                        case 1:
                            // Take feet inputs
                            Console.Write("Enter first feet value: ");
                            double f1 = Convert.ToDouble(Console.ReadLine());

                            Console.Write("Enter second feet value: ");
                            double f2 = Convert.ToDouble(Console.ReadLine());

                            // Call service method
                            bool feetResult = QuantityMeasurementService.AreFeetEqual(f1, f2);

                            Console.WriteLine("Feet Equal: " + feetResult);
                            break;

                        case 2:
                            // Take inches inputs
                            Console.Write("Enter first inches value: ");
                            double i1 = Convert.ToDouble(Console.ReadLine());

                            Console.Write("Enter second inches value: ");
                            double i2 = Convert.ToDouble(Console.ReadLine());

                            // Call service method
                            bool inchResult = QuantityMeasurementService.AreInchesEqual(i1, i2);

                            Console.WriteLine("Inches Equal: " + inchResult);
                            break;

                        case 3:
                            Console.WriteLine("Exiting application...");
                            break;

                        default:
                            Console.WriteLine("Invalid choice! Try again.");
                            break;
                    }
                }
                catch (FormatException)
                {
                    // Handle invalid numeric input
                    Console.WriteLine("Invalid input! Please enter numeric values.");
                }

            } while (choice != 3);
        }
    }
}