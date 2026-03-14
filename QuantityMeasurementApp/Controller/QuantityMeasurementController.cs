using QuantityMeasurementApp.BusinessLayer.Services;
using QuantityMeasurementApp.ModelLayer.DTO;
using QuantityMeasurementApp.ModelLayer.Models;
using QuantityMeasurementApp.RepoLayer.Repositories;

namespace QuantityMeasurementApp.Controller
{
    /// <summary>
    /// Handles user input and invokes quantity measurement operations.
    /// </summary>
    public class QuantityMeasurementController
    {
        private readonly QuantityMeasurementServiceImpl service;

        public QuantityMeasurementController()
        {
            var repository = QuantityMeasurementCacheRepository.GetInstance();
            service = new QuantityMeasurementServiceImpl(repository);
        }

        /// <summary>
        /// Displays main category menu.
        /// </summary>
        public void ShowMainMenu()
        {
            bool isRunning = true;
            while (isRunning)
            {
                System.Console.WriteLine("\n╔═══════════════════════════════════════╗");
                System.Console.WriteLine("║    QUANTITY MEASUREMENT APP     ║");
                System.Console.WriteLine("╠═══════════════════════════════════════╣");
                System.Console.WriteLine("║  1  Length Operations                 ║");
                System.Console.WriteLine("║  2  Weight Operations                 ║");
                System.Console.WriteLine("║  3  Volume Operations                 ║");
                System.Console.WriteLine("║  4  Temperature Operations            ║");
                System.Console.WriteLine("║  5  Exit                              ║");
                System.Console.WriteLine("╚═══════════════════════════════════════╝");

                int choice = ReadInteger("Select option: ");

                switch (choice)
                {
                    case 1: ShowLengthOperations(); break;
                    case 2: ShowWeightOperations(); break;
                    case 3: ShowVolumeOperations(); break;
                    case 4: ShowTemperatureOperations(); break;
                    case 5: isRunning = false; break;
                    default:
                        System.Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
            System.Console.WriteLine("\nGoodbye!");
        }

        // ── Length ────────────────────────────────────────────────────────

        private void ShowLengthOperations()
        {
            System.Console.WriteLine("\n╔═══════════════════════════════════════╗");
            System.Console.WriteLine("║         LENGTH OPERATIONS             ║");
            System.Console.WriteLine("╠═══════════════════════════════════════╣");
            System.Console.WriteLine("║  1  Compare Length                    ║");
            System.Console.WriteLine("║  2  Convert Length                    ║");
            System.Console.WriteLine("║  3  Add Length                        ║");
            System.Console.WriteLine("║  4  Subtract Length                   ║");
            System.Console.WriteLine("║  5  Divide Length                     ║");
            System.Console.WriteLine("╚═══════════════════════════════════════╝");

            int option = ReadInteger("Select operation: ");

            switch (option)
            {
                case 1: CompareLength(); break;
                case 2: ConvertLength(); break;
                case 3: AddLength(); break;
                case 4: SubtractLength(); break;
                case 5: DivideLength(); break;
                default:
                    System.Console.WriteLine("Invalid choice.");
                    break;
            }
        }

        private void CompareLength()
        {
            double firstValue = ReadDouble("Enter first length value: ");
            LengthEnum firstUnit = ReadLengthUnit();
            double secondValue = ReadDouble("Enter second length value: ");
            LengthEnum secondUnit = ReadLengthUnit();
            bool result = service.Compare(
                new QuantityDTO(firstValue, firstUnit),
                new QuantityDTO(secondValue, secondUnit));
            System.Console.WriteLine("Length Equal: " + result);
        }

        private void ConvertLength()
        {
            double value = ReadDouble("Enter length value: ");
            System.Console.WriteLine("From unit:");
            LengthEnum fromUnit = ReadLengthUnit();
            System.Console.WriteLine("To unit:");
            LengthEnum targetUnit = ReadLengthUnit();
            var result = service.Convert(new QuantityDTO(value, fromUnit), targetUnit);
            System.Console.WriteLine($"Converted Length: {result.Value} {result.Unit}");
        }

        private void AddLength()
        {
            double firstValue = ReadDouble("Enter first length: ");
            LengthEnum firstUnit = ReadLengthUnit();
            double secondValue = ReadDouble("Enter second length: ");
            LengthEnum secondUnit = ReadLengthUnit();
            System.Console.WriteLine("Result unit:");
            LengthEnum resultUnit = ReadLengthUnit();
            var result = service.Add(
                new QuantityDTO(firstValue, firstUnit),
                new QuantityDTO(secondValue, secondUnit),
                resultUnit);
            System.Console.WriteLine($"Length Result: {result.Value} {result.Unit}");
        }

        private void SubtractLength()
        {
            double firstValue = ReadDouble("Enter first length: ");
            LengthEnum firstUnit = ReadLengthUnit();
            double secondValue = ReadDouble("Enter second length: ");
            LengthEnum secondUnit = ReadLengthUnit();
            var result = service.Subtract(
                new QuantityDTO(firstValue, firstUnit),
                new QuantityDTO(secondValue, secondUnit));
            System.Console.WriteLine($"Result: {result.Value} {result.Unit}");
        }

        private void DivideLength()
        {
            double firstValue = ReadDouble("Enter first length: ");
            LengthEnum firstUnit = ReadLengthUnit();
            double secondValue = ReadDouble("Enter second length: ");
            LengthEnum secondUnit = ReadLengthUnit();
            double result = service.Divide(
                new QuantityDTO(firstValue, firstUnit),
                new QuantityDTO(secondValue, secondUnit));
            System.Console.WriteLine($"Division Result: {result}");
        }

        // ── Weight ────────────────────────────────────────────────────────

        private void ShowWeightOperations()
        {
            System.Console.WriteLine("\n╔═══════════════════════════════════════╗");
            System.Console.WriteLine("║         WEIGHT OPERATIONS             ║");
            System.Console.WriteLine("╠═══════════════════════════════════════╣");
            System.Console.WriteLine("║  1  Compare Weight                    ║");
            System.Console.WriteLine("║  2  Convert Weight                    ║");
            System.Console.WriteLine("║  3  Add Weight                        ║");
            System.Console.WriteLine("║  4  Subtract Weight                   ║");
            System.Console.WriteLine("║  5  Divide Weight                     ║");
            System.Console.WriteLine("╚═══════════════════════════════════════╝");

            int option = ReadInteger("Select operation: ");

            switch (option)
            {
                case 1: CompareWeight(); break;
                case 2: ConvertWeight(); break;
                case 3: AddWeight(); break;
                case 4: SubtractWeight(); break;
                case 5: DivideWeight(); break;
                default:
                    System.Console.WriteLine("Invalid choice.");
                    break;
            }
        }

        private void CompareWeight()
        {
            double firstValue = ReadDouble("Enter first weight: ");
            WeightEnum firstUnit = ReadWeightUnit();
            double secondValue = ReadDouble("Enter second weight: ");
            WeightEnum secondUnit = ReadWeightUnit();
            bool result = service.Compare(
                new QuantityDTO(firstValue, firstUnit),
                new QuantityDTO(secondValue, secondUnit));
            System.Console.WriteLine("Weight Equal: " + result);
        }

        private void ConvertWeight()
        {
            double value = ReadDouble("Enter weight value: ");
            System.Console.WriteLine("From unit:");
            WeightEnum fromUnit = ReadWeightUnit();
            System.Console.WriteLine("To unit:");
            WeightEnum targetUnit = ReadWeightUnit();
            var result = service.Convert(new QuantityDTO(value, fromUnit), targetUnit);
            System.Console.WriteLine($"Converted Weight: {result.Value} {result.Unit}");
        }

        private void AddWeight()
        {
            double firstValue = ReadDouble("Enter first weight: ");
            WeightEnum firstUnit = ReadWeightUnit();
            double secondValue = ReadDouble("Enter second weight: ");
            WeightEnum secondUnit = ReadWeightUnit();
            System.Console.WriteLine("Result unit:");
            WeightEnum resultUnit = ReadWeightUnit();
            var result = service.Add(
                new QuantityDTO(firstValue, firstUnit),
                new QuantityDTO(secondValue, secondUnit),
                resultUnit);
            System.Console.WriteLine($"Weight Result: {result.Value} {result.Unit}");
        }

        private void SubtractWeight()
        {
            double firstValue = ReadDouble("Enter first weight: ");
            WeightEnum firstUnit = ReadWeightUnit();
            double secondValue = ReadDouble("Enter second weight: ");
            WeightEnum secondUnit = ReadWeightUnit();
            var result = service.Subtract(
                new QuantityDTO(firstValue, firstUnit),
                new QuantityDTO(secondValue, secondUnit));
            System.Console.WriteLine($"Result: {result.Value} {result.Unit}");
        }

        private void DivideWeight()
        {
            double firstValue = ReadDouble("Enter first weight: ");
            WeightEnum firstUnit = ReadWeightUnit();
            double secondValue = ReadDouble("Enter second weight: ");
            WeightEnum secondUnit = ReadWeightUnit();
            double result = service.Divide(
                new QuantityDTO(firstValue, firstUnit),
                new QuantityDTO(secondValue, secondUnit));
            System.Console.WriteLine($"Division Result: {result}");
        }

        // ── Volume ────────────────────────────────────────────────────────

        private void ShowVolumeOperations()
        {
            System.Console.WriteLine("\n╔═══════════════════════════════════════╗");
            System.Console.WriteLine("║         VOLUME OPERATIONS             ║");
            System.Console.WriteLine("╠═══════════════════════════════════════╣");
            System.Console.WriteLine("║  1  Compare Volume                    ║");
            System.Console.WriteLine("║  2  Convert Volume                    ║");
            System.Console.WriteLine("║  3  Add Volume                        ║");
            System.Console.WriteLine("║  4  Subtract Volume                   ║");
            System.Console.WriteLine("║  5  Divide Volume                     ║");
            System.Console.WriteLine("╚═══════════════════════════════════════╝");

            int option = ReadInteger("Select operation: ");

            switch (option)
            {
                case 1: CompareVolume(); break;
                case 2: ConvertVolume(); break;
                case 3: AddVolume(); break;
                case 4: SubtractVolume(); break;
                case 5: DivideVolume(); break;
                default:
                    System.Console.WriteLine("Invalid choice.");
                    break;
            }
        }

        private void CompareVolume()
        {
            double firstValue = ReadDouble("Enter first volume: ");
            VolumeEnum firstUnit = ReadVolumeUnit();
            double secondValue = ReadDouble("Enter second volume: ");
            VolumeEnum secondUnit = ReadVolumeUnit();
            bool result = service.Compare(
                new QuantityDTO(firstValue, firstUnit),
                new QuantityDTO(secondValue, secondUnit));
            System.Console.WriteLine("Volume Equal: " + result);
        }

        private void ConvertVolume()
        {
            double value = ReadDouble("Enter volume value: ");
            System.Console.WriteLine("From unit:");
            VolumeEnum fromUnit = ReadVolumeUnit();
            System.Console.WriteLine("To unit:");
            VolumeEnum targetUnit = ReadVolumeUnit();
            var result = service.Convert(new QuantityDTO(value, fromUnit), targetUnit);
            System.Console.WriteLine($"Converted Volume: {result.Value} {result.Unit}");
        }

        private void AddVolume()
        {
            double firstValue = ReadDouble("Enter first volume: ");
            VolumeEnum firstUnit = ReadVolumeUnit();
            double secondValue = ReadDouble("Enter second volume: ");
            VolumeEnum secondUnit = ReadVolumeUnit();
            System.Console.WriteLine("Result unit:");
            VolumeEnum resultUnit = ReadVolumeUnit();
            var result = service.Add(
                new QuantityDTO(firstValue, firstUnit),
                new QuantityDTO(secondValue, secondUnit),
                resultUnit);
            System.Console.WriteLine($"Volume Result: {result.Value} {result.Unit}");
        }

        private void SubtractVolume()
        {
            double firstValue = ReadDouble("Enter first volume: ");
            VolumeEnum firstUnit = ReadVolumeUnit();
            double secondValue = ReadDouble("Enter second volume: ");
            VolumeEnum secondUnit = ReadVolumeUnit();
            var result = service.Subtract(
                new QuantityDTO(firstValue, firstUnit),
                new QuantityDTO(secondValue, secondUnit));
            System.Console.WriteLine($"Result: {result.Value} {result.Unit}");
        }

        private void DivideVolume()
        {
            double firstValue = ReadDouble("Enter first volume: ");
            VolumeEnum firstUnit = ReadVolumeUnit();
            double secondValue = ReadDouble("Enter second volume: ");
            VolumeEnum secondUnit = ReadVolumeUnit();
            double result = service.Divide(
                new QuantityDTO(firstValue, firstUnit),
                new QuantityDTO(secondValue, secondUnit));
            System.Console.WriteLine($"Division Result: {result}");
        }

        // ── Temperature ───────────────────────────────────────────────────

        private void ShowTemperatureOperations()
        {
            System.Console.WriteLine("\n╔═══════════════════════════════════════╗");
            System.Console.WriteLine("║       TEMPERATURE OPERATIONS          ║");
            System.Console.WriteLine("╠═══════════════════════════════════════╣");
            System.Console.WriteLine("║  1  Compare Temperature               ║");
            System.Console.WriteLine("║  2  Convert Temperature               ║");
            System.Console.WriteLine("╚═══════════════════════════════════════╝");

            int option = ReadInteger("Select operation: ");

            switch (option)
            {
                case 1: CompareTemperature(); break;
                case 2: ConvertTemperature(); break;
                default:
                    System.Console.WriteLine("Invalid choice.");
                    break;
            }
        }

        private void CompareTemperature()
        {
            double firstValue = ReadDouble("Enter temperature value: ");
            double secondValue = ReadDouble("Enter second temperature: ");
            var first = new Quantity<TemperatureEnum>(firstValue, TemperatureEnum.CELSIUS);
            var second = new Quantity<TemperatureEnum>(secondValue, TemperatureEnum.FAHRENHEIT);
            System.Console.WriteLine("Temperature Equal: " + first.Equals(second));
        }

        private void ConvertTemperature()
        {
            double value = ReadDouble("Enter temperature value: ");
            System.Console.WriteLine("From unit:");
            TemperatureEnum fromUnit = ReadTemperatureUnit();
            System.Console.WriteLine("To unit:");
            TemperatureEnum targetUnit = ReadTemperatureUnit();
            var result = service.Convert(new QuantityDTO(value, fromUnit), targetUnit);
            System.Console.WriteLine($"Converted Temperature: {result.Value} {result.Unit}");
        }

        // ── Helpers ───────────────────────────────────────────────────────

        private double ReadDouble(string message)
        {
            System.Console.Write(message);
            return double.Parse(System.Console.ReadLine() ?? "0");
        }

        private int ReadInteger(string message)
        {
            System.Console.Write(message);
            return int.Parse(System.Console.ReadLine() ?? "0");
        }

        private LengthEnum ReadLengthUnit()
        {
            System.Console.WriteLine("1 FEET");
            System.Console.WriteLine("2 INCHES");
            System.Console.WriteLine("3 YARDS");
            System.Console.WriteLine("4 CENTIMETERS");
            return (LengthEnum)(ReadInteger("Choose unit: ") - 1);
        }

        private WeightEnum ReadWeightUnit()
        {
            System.Console.WriteLine("1 KILOGRAM");
            System.Console.WriteLine("2 GRAM");
            System.Console.WriteLine("3 POUND");
            return (WeightEnum)(ReadInteger("Choose unit: ") - 1);
        }

        private VolumeEnum ReadVolumeUnit()
        {
            System.Console.WriteLine("1 LITRE");
            System.Console.WriteLine("2 MILLILITRE");
            System.Console.WriteLine("3 GALLON");
            return (VolumeEnum)(ReadInteger("Choose unit: ") - 1);
        }

        private TemperatureEnum ReadTemperatureUnit()
        {
            System.Console.WriteLine("1 CELSIUS");
            System.Console.WriteLine("2 FAHRENHEIT");
            System.Console.WriteLine("3 KELVIN");
            return (TemperatureEnum)(ReadInteger("Choose unit: ") - 1);
        }
    }
}