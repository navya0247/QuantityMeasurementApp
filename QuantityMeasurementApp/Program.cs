using System;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Services;

namespace QuantityMeasurementApp
{
    public class QuantityMeasurementApp
    {
        public static void Main(string[] args)
        {
            QuantityMeasurementService service = new QuantityMeasurementService();

            try
            {
                // Take first input
                Console.Write("Enter first value in feet: ");
                double value1 = Convert.ToDouble(Console.ReadLine());

                // Take second input
                Console.Write("Enter second value in feet: ");
                double value2 = Convert.ToDouble(Console.ReadLine());

                // Create Feet objects
                Feet feet1 = new Feet(value1);
                Feet feet2 = new Feet(value2);

                // Call service to compare
                bool result = service.AreFeetEqual(feet1, feet2);

                // Display result
                Console.WriteLine("Equal: " + result);
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input. Please enter numeric values.");
            }
        }
    }
}