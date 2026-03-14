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
                System.Console.WriteLine("\n===== Quantity Measurement Menu =====");
                System.Console.WriteLine("1 Length Operations");
                System.Console.WriteLine("2 Weight Operations");
                System.Console.WriteLine("3 Volume Operations");
                System.Console.WriteLine("4 Temperature Operations");
                System.Console.WriteLine("5 Exit");

                int choice = ReadInteger("Select option: ");

                switch (choice)
                {
                    case 1:
                        ShowLengthOperations();
                        break;
                    case 2:
                        ShowWeightOperations();
                        break;
                    case 3:
                        ShowVolumeOperations();
                        break;
                    case 4:
                        CompareTemperature();
                        break;
                    case 5:
                        isRunning = false;
                        break;
                    default:
                        System.Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }

        private void ShowLengthOperations()
        {
            System.Console.WriteLine("\n1 Compare Length");
            System.Console.WriteLine("2 Convert Length");
            System.Console.WriteLine("3 Add Length");
            System.Console.WriteLine("4 Subtract Length");
            System.Console.WriteLine("5 Divide Length");

            int option = ReadInteger("Select operation: ");

            switch (option)
            {
                case 1: CompareLength(); break;
                case 2: ConvertLength(); break;
                case 3: AddLength(); break;
                case 4: SubtractLength(); break;
                case 5: DivideLength(); break;
            }
        }

        private void ShowWeightOperations()
        {
            System.Console.WriteLine("\n1 Compare Weight");
            System.Console.WriteLine("2 Convert Weight");
            System.Console.WriteLine("3 Add Weight");

            int option = ReadInteger("Select operation: ");

            switch (option)
            {
                case 1: CompareWeight(); break;
                case 2: ConvertWeight(); break;
                case 3: AddWeight(); break;
            }
        }

        private void ShowVolumeOperations()
        {
            System.Console.WriteLine("\n1 Compare Volume");
            System.Console.WriteLine("2 Convert Volume");
            System.Console.WriteLine("3 Add Volume");

            int option = ReadInteger("Select operation: ");

            switch (option)
            {
                case 1: CompareVolume(); break;
                case 2: ConvertVolume(); break;
                case 3: AddVolume(); break;
            }
        }

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

        private void CompareLength()
        {
            double firstValue = ReadDouble("Enter first length value: ");
            LengthEnum firstUnit = ReadLengthUnit();

            double secondValue = ReadDouble("Enter second length value: ");
            LengthEnum secondUnit = ReadLengthUnit();

            var firstQuantity = new QuantityDTO(firstValue, firstUnit);
            var secondQuantity = new QuantityDTO(secondValue, secondUnit);

            bool result = service.Compare(firstQuantity, secondQuantity);

            System.Console.WriteLine("Length Equal: " + result);
        }

        private void ConvertLength()
        {
            double value = ReadDouble("Enter length value: ");
            LengthEnum fromUnit = ReadLengthUnit();
            LengthEnum targetUnit = ReadLengthUnit();

            var quantity = new QuantityDTO(value, fromUnit);
            var result = service.Convert(quantity, targetUnit);

            System.Console.WriteLine($"Converted Length: {result.Value} {result.Unit}");
        }

        private void AddLength()
        {
            double firstValue = ReadDouble("Enter first length: ");
            LengthEnum firstUnit = ReadLengthUnit();

            double secondValue = ReadDouble("Enter second length: ");
            LengthEnum secondUnit = ReadLengthUnit();

            LengthEnum resultUnit = ReadLengthUnit();

            var firstQuantity = new QuantityDTO(firstValue, firstUnit);
            var secondQuantity = new QuantityDTO(secondValue, secondUnit);

            var result = service.Add(firstQuantity, secondQuantity, resultUnit);

            System.Console.WriteLine($"Length Result: {result.Value} {result.Unit}");
        }

        private void SubtractLength()
        {
            double firstValue = ReadDouble("Enter first length: ");
            LengthEnum firstUnit = ReadLengthUnit();

            double secondValue = ReadDouble("Enter second length: ");
            LengthEnum secondUnit = ReadLengthUnit();

            var firstQuantity = new QuantityDTO(firstValue, firstUnit);
            var secondQuantity = new QuantityDTO(secondValue, secondUnit);

            var result = service.Subtract(firstQuantity, secondQuantity);

            System.Console.WriteLine($"Result: {result.Value} {result.Unit}");
        }

        private void DivideLength()
        {
            double firstValue = ReadDouble("Enter first length: ");
            LengthEnum firstUnit = ReadLengthUnit();

            double secondValue = ReadDouble("Enter second length: ");
            LengthEnum secondUnit = ReadLengthUnit();

            var firstQuantity = new QuantityDTO(firstValue, firstUnit);
            var secondQuantity = new QuantityDTO(secondValue, secondUnit);

            double result = service.Divide(firstQuantity, secondQuantity);

            System.Console.WriteLine($"Division Result: {result}");
        }

        private void CompareWeight()
        {
            double firstValue = ReadDouble("Enter first weight: ");
            WeightEnum firstUnit = ReadWeightUnit();

            double secondValue = ReadDouble("Enter second weight: ");
            WeightEnum secondUnit = ReadWeightUnit();

            var firstQuantity = new QuantityDTO(firstValue, firstUnit);
            var secondQuantity = new QuantityDTO(secondValue, secondUnit);

            bool result = service.Compare(firstQuantity, secondQuantity);

            System.Console.WriteLine("Weight Equal: " + result);
        }

        private void ConvertWeight()
        {
            double value = ReadDouble("Enter weight value: ");
            WeightEnum fromUnit = ReadWeightUnit();
            WeightEnum targetUnit = ReadWeightUnit();

            var quantity = new QuantityDTO(value, fromUnit);
            var result = service.Convert(quantity, targetUnit);

            System.Console.WriteLine($"Converted Weight: {result.Value} {result.Unit}");
        }

        private void AddWeight()
        {
            double firstValue = ReadDouble("Enter first weight: ");
            WeightEnum firstUnit = ReadWeightUnit();

            double secondValue = ReadDouble("Enter second weight: ");
            WeightEnum secondUnit = ReadWeightUnit();

            WeightEnum resultUnit = ReadWeightUnit();

            var firstQuantity = new QuantityDTO(firstValue, firstUnit);
            var secondQuantity = new QuantityDTO(secondValue, secondUnit);

            var result = service.Add(firstQuantity, secondQuantity, resultUnit);

            System.Console.WriteLine($"Weight Result: {result.Value} {result.Unit}");
        }

        private void CompareVolume()
        {
            double firstValue = ReadDouble("Enter first volume: ");
            VolumeEnum firstUnit = ReadVolumeUnit();

            double secondValue = ReadDouble("Enter second volume: ");
            VolumeEnum secondUnit = ReadVolumeUnit();

            var firstQuantity = new QuantityDTO(firstValue, firstUnit);
            var secondQuantity = new QuantityDTO(secondValue, secondUnit);

            bool result = service.Compare(firstQuantity, secondQuantity);

            System.Console.WriteLine("Volume Equal: " + result);
        }

        private void ConvertVolume()
        {
            double value = ReadDouble("Enter volume value: ");
            VolumeEnum fromUnit = ReadVolumeUnit();
            VolumeEnum targetUnit = ReadVolumeUnit();

            var quantity = new QuantityDTO(value, fromUnit);
            var result = service.Convert(quantity, targetUnit);

            System.Console.WriteLine($"Converted Volume: {result.Value} {result.Unit}");
        }

        private void AddVolume()
        {
            double firstValue = ReadDouble("Enter first volume: ");
            VolumeEnum firstUnit = ReadVolumeUnit();

            double secondValue = ReadDouble("Enter second volume: ");
            VolumeEnum secondUnit = ReadVolumeUnit();

            VolumeEnum resultUnit = ReadVolumeUnit();

            var firstQuantity = new QuantityDTO(firstValue, firstUnit);
            var secondQuantity = new QuantityDTO(secondValue, secondUnit);

            var result = service.Add(firstQuantity, secondQuantity, resultUnit);

            System.Console.WriteLine($"Volume Result: {result.Value} {result.Unit}");
        }

        private void CompareTemperature()
        {
            double firstValue = ReadDouble("Enter temperature value: ");
            double secondValue = ReadDouble("Enter second temperature: ");

            var first = new Quantity<TemperatureEnum>(firstValue, TemperatureEnum.CELSIUS);
            var second = new Quantity<TemperatureEnum>(secondValue, TemperatureEnum.FAHRENHEIT);

            System.Console.WriteLine("Temperature Equal: " + first.Equals(second));
        }
    }


}
