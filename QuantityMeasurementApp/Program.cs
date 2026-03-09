using System;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp
{
    /// <summary>
    /// UC10 Implementation
    /// Refactored application to use Generic Quantity<U> class.
    /// All UC1–UC9 functionality is preserved while using the generic design.
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
                Console.WriteLine("10. Exit");

                Console.Write("Enter choice: ");

                if (!int.TryParse(Console.ReadLine(), out choice))
                {
                    Console.WriteLine("Invalid input!");
                    continue;
                }

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

                            var feet1 = new Quantity<LengthUnit>(f1, LengthUnit.FEET);
                            var feet2 = new Quantity<LengthUnit>(f2, LengthUnit.FEET);

                            Console.WriteLine("Equal: " + feet1.Equals(feet2));

                            break;

                        // ---------- UC2 ----------
                        case 2:

                            Console.Write("Enter first inches value: ");
                            double i1 = Convert.ToDouble(Console.ReadLine());

                            Console.Write("Enter second inches value: ");
                            double i2 = Convert.ToDouble(Console.ReadLine());

                            var inch1 = new Quantity<LengthUnit>(i1, LengthUnit.INCHES);
                            var inch2 = new Quantity<LengthUnit>(i2, LengthUnit.INCHES);

                            Console.WriteLine("Equal: " + inch1.Equals(inch2));

                            break;

                        // ---------- UC3 / UC4 ----------
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

                            var quantity = new Quantity<LengthUnit>(value, from);

                            Console.WriteLine("Converted Value: " + quantity.ConvertTo(to));

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

                            var add1 = new Quantity<LengthUnit>(a1, ua1);
                            var add2 = new Quantity<LengthUnit>(a2, ua2);

                            var result = add1.Add(add2, ua1);

                            Console.WriteLine($"Result: {result}");

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

                            var addA = new Quantity<LengthUnit>(b1, ub1);
                            var addB = new Quantity<LengthUnit>(b2, ub2);

                            var resultUC7 = addA.Add(addB, target);

                            Console.WriteLine($"Result: {resultUC7}");

                            break;

                        // ---------- UC8 ----------
                        case 7:

                            Console.Write("Enter value: ");
                            double inputValue = Convert.ToDouble(Console.ReadLine());

                            Console.WriteLine("Select Unit:");
                            DisplayUnits();
                            LengthUnit selectedUnit = (LengthUnit)(Convert.ToInt32(Console.ReadLine()) - 1);

                            double baseValue = selectedUnit.ConvertToBaseUnit(inputValue);

                            Console.WriteLine($"Converted to FEET: {baseValue}");

                            break;

                        // ---------- UC9 ----------
                        case 8:

                            Console.Write("Enter first weight value: ");
                            double w1 = Convert.ToDouble(Console.ReadLine());

                            Console.WriteLine("Select Unit:");
                            DisplayWeightUnits();
                            WeightUnit wu1 = (WeightUnit)(Convert.ToInt32(Console.ReadLine()) - 1);

                            Console.Write("Enter second weight value: ");
                            double w2 = Convert.ToDouble(Console.ReadLine());

                            Console.WriteLine("Select Unit:");
                            DisplayWeightUnits();
                            WeightUnit wu2 = (WeightUnit)(Convert.ToInt32(Console.ReadLine()) - 1);

                            var weight1 = new Quantity<WeightUnit>(w1, wu1);
                            var weight2 = new Quantity<WeightUnit>(w2, wu2);

                            var weightSum = weight1.Add(weight2, wu1);

                            Console.WriteLine($"Result: {weightSum}");

                            break;

                        // ---------- UC10 ----------
                        case 9:

                            Console.WriteLine("\n--- UC10 Generic Quantity Demonstration ---");

                            var length1 = new Quantity<LengthUnit>(1, LengthUnit.FEET);
                            var length2 = new Quantity<LengthUnit>(12, LengthUnit.INCHES);

                            Console.WriteLine($"1 FEET == 12 INCHES → {length1.Equals(length2)}");

                            var lengthConverted = length1.ConvertTo(LengthUnit.INCHES);
                            Console.WriteLine($"1 FEET in INCHES → {lengthConverted}");

                            var lengthSum = length1.Add(length2, LengthUnit.FEET);
                            Console.WriteLine($"1 FEET + 12 INCHES → {lengthSum}");

                            var weightA = new Quantity<WeightUnit>(1, WeightUnit.KILOGRAM);
                            var weightB = new Quantity<WeightUnit>(1000, WeightUnit.GRAM);

                            Console.WriteLine($"1 KG == 1000 GRAM → {weightA.Equals(weightB)}");

                            var weightConverted = weightA.ConvertTo(WeightUnit.GRAM);
                            Console.WriteLine($"1 KG in GRAM → {weightConverted}");

                            var weightAdd = weightA.Add(weightB, WeightUnit.KILOGRAM);
                            Console.WriteLine($"1 KG + 1000 GRAM → {weightAdd}");

                            break;

                        case 10:
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

            } while (choice != 10);
        }

        private static void DisplayUnits()
        {
            Console.WriteLine("1 = FEET");
            Console.WriteLine("2 = INCHES");
            Console.WriteLine("3 = YARDS");
            Console.WriteLine("4 = CENTIMETERS");
        }

        private static void DisplayWeightUnits()
        {
            Console.WriteLine("1 = KILOGRAM");
            Console.WriteLine("2 = GRAM");
            Console.WriteLine("3 = POUND");
        }
    }
}