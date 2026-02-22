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
                        case 1:
                            Console.Write("Enter first feet value: ");
                            double f1 = Convert.ToDouble(Console.ReadLine());
                            Console.Write("Enter second feet value: ");
                            double f2 = Convert.ToDouble(Console.ReadLine());

                            Console.WriteLine("Equal: " +
                                QuantityMeasurementService.AreFeetEqual(f1, f2));
                            break;

                        case 2:
                            Console.Write("Enter first inches value: ");
                            double i1 = Convert.ToDouble(Console.ReadLine());
                            Console.Write("Enter second inches value: ");
                            double i2 = Convert.ToDouble(Console.ReadLine());

                            Console.WriteLine("Equal: " +
                                QuantityMeasurementService.AreInchesEqual(i1, i2));
                            break;

                        case 3:
                            Console.Write("Enter first value: ");
                            double v1 = Convert.ToDouble(Console.ReadLine());
                            Console.WriteLine("Unit: 1=Feet 2=Inch");
                            LengthUnit u1 = (LengthUnit)(Convert.ToInt32(Console.ReadLine()) - 1);

                            Console.Write("Enter second value: ");
                            double v2 = Convert.ToDouble(Console.ReadLine());
                            Console.WriteLine("Unit: 1=Feet 2=Inch");
                            LengthUnit u2 = (LengthUnit)(Convert.ToInt32(Console.ReadLine()) - 1);

                            Console.WriteLine("Equal: " +
                                QuantityMeasurementService.AreLengthEqual(v1, u1, v2, u2));
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