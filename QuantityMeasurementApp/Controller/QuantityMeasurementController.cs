using QuantityMeasurementApp.BusinessLayer.Interfaces;
using QuantityMeasurementApp.BusinessLayer.Services;
using QuantityMeasurementApp.ModelLayer.DTO;
using QuantityMeasurementApp.ModelLayer.Models;
using QuantityMeasurementApp.RepoLayer.Repositories;
using System;

namespace QuantityMeasurementApp.Controller
{
    public class QuantityMeasurementController
    {
        private readonly IQuantityMeasurementService service;

        public QuantityMeasurementController()
        {
            var repository = new QuantityMeasurementDatabaseRepository();
            service = new QuantityMeasurementServiceImpl(repository);
        }

        public void ShowMainMenu()
        {
            bool isRunning = true;
            while (isRunning)
            {
                Console.WriteLine("\n===== Quantity Measurement Menu =====");
                Console.WriteLine("1. Length Operations");
                Console.WriteLine("2. Weight Operations");
                Console.WriteLine("3. Volume Operations");
                Console.WriteLine("4. Temperature Operations");
                Console.WriteLine("5. History & Statistics");
                Console.WriteLine("6. Exit");
                int choice = ReadInteger("Select option: ");
                switch (choice)
                {
                    case 1: ShowLengthOperations(); break;
                    case 2: ShowWeightOperations(); break;
                    case 3: ShowVolumeOperations(); break;
                    case 4: CompareTemperature();   break;
                    case 5: ShowHistoryMenu();       break;
                    case 6: isRunning = false;       break;
                    default: Console.WriteLine("Invalid choice."); break;
                }
            }
        }

        // ── History & Statistics ──────────────────────────────────────────

        private void ShowHistoryMenu()
        {
            Console.WriteLine("\n===== History & Statistics =====");
            Console.WriteLine("1. View all history");
            Console.WriteLine("2. View by operation type");
            Console.WriteLine("3. View by measurement type");
            Console.WriteLine("4. View statistics");
            Console.WriteLine("5. Clear all records");
            int option = ReadInteger("Select option: ");
            switch (option)
            {
                case 1: ViewAllHistory();        break;
                case 2: ViewByOperation();       break;
                case 3: ViewByMeasurementType(); break;
                case 4: ViewStatistics();        break;
                case 5: ClearAllRecords();       break;
                default: Console.WriteLine("Invalid choice."); break;
            }
        }

        private void ViewAllHistory()
        {
            var records = service.GetAllMeasurements();
            Console.WriteLine($"\n--- All History ({records.Count} records) ---");
            foreach (var r in records)
                Console.WriteLine(
                    $"[{r.MeasureType}] {r.Operation}: {r.OperandOne} | {r.OperandTwo} => {r.Result}");
        }

        private void ViewByOperation()
        {
            Console.Write("Enter operation (COMPARE/ADD/SUBTRACT/DIVIDE/CONVERT): ");
            string op = Console.ReadLine()?.Trim().ToUpper() ?? "";
            var records = service.GetByOperation(op);
            Console.WriteLine($"\n--- {op} ({records.Count} records) ---");
            foreach (var r in records)
                Console.WriteLine(
                    $"[{r.MeasureType}] {r.OperandOne} | {r.OperandTwo} => {r.Result}");
        }

        private void ViewByMeasurementType()
        {
            Console.Write("Enter type (LENGTH/WEIGHT/VOLUME/TEMPERATURE): ");
            string mt = Console.ReadLine()?.Trim().ToUpper() ?? "";
            var records = service.GetByMeasureType(mt);
            Console.WriteLine($"\n--- {mt} ({records.Count} records) ---");
            foreach (var r in records)
                Console.WriteLine(
                    $"[{r.Operation}] {r.OperandOne} | {r.OperandTwo} => {r.Result}");
        }

        private void ViewStatistics()
        {
            Console.WriteLine("\n--- Statistics ---");
            Console.WriteLine($"Total records : {service.GetTotalCount()}");
            Console.WriteLine($"Pool info     : {service.GetPoolStats()}");
        }

        private void ClearAllRecords()
        {
            Console.Write("Are you sure you want to clear all records? (yes/no): ");
            if (Console.ReadLine()?.Trim().ToLower() == "yes")
            {
                service.DeleteAll();
                Console.WriteLine("All records cleared successfully.");
            }
        }

        // ── Length ────────────────────────────────────────────────────────

        private void ShowLengthOperations()
        {
            Console.WriteLine("\n1. Compare Length");
            Console.WriteLine("2. Convert Length");
            Console.WriteLine("3. Add Length");
            Console.WriteLine("4. Subtract Length");
            Console.WriteLine("5. Divide Length");
            int option = ReadInteger("Select operation: ");
            switch (option)
            {
                case 1: CompareLength();  break;
                case 2: ConvertLength();  break;
                case 3: AddLength();      break;
                case 4: SubtractLength(); break;
                case 5: DivideLength();   break;
            }
        }

        private void CompareLength()
        {
            double v1 = ReadDouble("Enter first length value: ");
            LengthEnum u1 = ReadLengthUnit();
            double v2 = ReadDouble("Enter second length value: ");
            LengthEnum u2 = ReadLengthUnit();
            bool result = service.Compare(
                new QuantityDTO(v1, u1), new QuantityDTO(v2, u2));
            Console.WriteLine("Length Equal: " + result);
        }

        private void ConvertLength()
        {
            double v = ReadDouble("Enter length value: ");
            LengthEnum from = ReadLengthUnit();
            LengthEnum to   = ReadLengthUnit();
            var result = service.Convert(new QuantityDTO(v, from), to);
            Console.WriteLine($"Converted Length: {result.Value} {result.Unit}");
        }

        private void AddLength()
        {
            double v1 = ReadDouble("Enter first length: ");
            LengthEnum u1 = ReadLengthUnit();
            double v2 = ReadDouble("Enter second length: ");
            LengthEnum u2 = ReadLengthUnit();
            LengthEnum target = ReadLengthUnit();
            var result = service.Add(
                new QuantityDTO(v1, u1), new QuantityDTO(v2, u2), target);
            Console.WriteLine($"Length Result: {result.Value} {result.Unit}");
        }

        private void SubtractLength()
        {
            double v1 = ReadDouble("Enter first length: ");
            LengthEnum u1 = ReadLengthUnit();
            double v2 = ReadDouble("Enter second length: ");
            LengthEnum u2 = ReadLengthUnit();
            var result = service.Subtract(
                new QuantityDTO(v1, u1), new QuantityDTO(v2, u2));
            Console.WriteLine($"Result: {result.Value} {result.Unit}");
        }

        private void DivideLength()
        {
            double v1 = ReadDouble("Enter first length: ");
            LengthEnum u1 = ReadLengthUnit();
            double v2 = ReadDouble("Enter second length: ");
            LengthEnum u2 = ReadLengthUnit();
            double result = service.Divide(
                new QuantityDTO(v1, u1), new QuantityDTO(v2, u2));
            Console.WriteLine($"Division Result: {result}");
        }

        // ── Weight ────────────────────────────────────────────────────────

        private void ShowWeightOperations()
        {
            Console.WriteLine("\n1. Compare Weight");
            Console.WriteLine("2. Convert Weight");
            Console.WriteLine("3. Add Weight");
            int option = ReadInteger("Select operation: ");
            switch (option)
            {
                case 1: CompareWeight(); break;
                case 2: ConvertWeight(); break;
                case 3: AddWeight();     break;
            }
        }

        private void CompareWeight()
        {
            double v1 = ReadDouble("Enter first weight: ");
            WeightEnum u1 = ReadWeightUnit();
            double v2 = ReadDouble("Enter second weight: ");
            WeightEnum u2 = ReadWeightUnit();
            bool result = service.Compare(
                new QuantityDTO(v1, u1), new QuantityDTO(v2, u2));
            Console.WriteLine("Weight Equal: " + result);
        }

        private void ConvertWeight()
        {
            double v = ReadDouble("Enter weight value: ");
            WeightEnum from = ReadWeightUnit();
            WeightEnum to   = ReadWeightUnit();
            var result = service.Convert(new QuantityDTO(v, from), to);
            Console.WriteLine($"Converted Weight: {result.Value} {result.Unit}");
        }

        private void AddWeight()
        {
            double v1 = ReadDouble("Enter first weight: ");
            WeightEnum u1 = ReadWeightUnit();
            double v2 = ReadDouble("Enter second weight: ");
            WeightEnum u2 = ReadWeightUnit();
            WeightEnum target = ReadWeightUnit();
            var result = service.Add(
                new QuantityDTO(v1, u1), new QuantityDTO(v2, u2), target);
            Console.WriteLine($"Weight Result: {result.Value} {result.Unit}");
        }

        // ── Volume ────────────────────────────────────────────────────────

        private void ShowVolumeOperations()
        {
            Console.WriteLine("\n1. Compare Volume");
            Console.WriteLine("2. Convert Volume");
            Console.WriteLine("3. Add Volume");
            int option = ReadInteger("Select operation: ");
            switch (option)
            {
                case 1: CompareVolume(); break;
                case 2: ConvertVolume(); break;
                case 3: AddVolume();     break;
            }
        }

        private void CompareVolume()
        {
            double v1 = ReadDouble("Enter first volume: ");
            VolumeEnum u1 = ReadVolumeUnit();
            double v2 = ReadDouble("Enter second volume: ");
            VolumeEnum u2 = ReadVolumeUnit();
            bool result = service.Compare(
                new QuantityDTO(v1, u1), new QuantityDTO(v2, u2));
            Console.WriteLine("Volume Equal: " + result);
        }

        private void ConvertVolume()
        {
            double v = ReadDouble("Enter volume value: ");
            VolumeEnum from = ReadVolumeUnit();
            VolumeEnum to   = ReadVolumeUnit();
            var result = service.Convert(new QuantityDTO(v, from), to);
            Console.WriteLine($"Converted Volume: {result.Value} {result.Unit}");
        }

        private void AddVolume()
        {
            double v1 = ReadDouble("Enter first volume: ");
            VolumeEnum u1 = ReadVolumeUnit();
            double v2 = ReadDouble("Enter second volume: ");
            VolumeEnum u2 = ReadVolumeUnit();
            VolumeEnum target = ReadVolumeUnit();
            var result = service.Add(
                new QuantityDTO(v1, u1), new QuantityDTO(v2, u2), target);
            Console.WriteLine($"Volume Result: {result.Value} {result.Unit}");
        }

        // ── Temperature ───────────────────────────────────────────────────

        private void CompareTemperature()
        {
            double v1 = ReadDouble("Enter first temperature value: ");
            double v2 = ReadDouble("Enter second temperature value: ");
            var first  = new Quantity<TemperatureEnum>(v1, TemperatureEnum.CELSIUS);
            var second = new Quantity<TemperatureEnum>(v2, TemperatureEnum.FAHRENHEIT);
            Console.WriteLine("Temperature Equal: " + first.Equals(second));
        }

        // ── Helpers ───────────────────────────────────────────────────────

        private double ReadDouble(string message)
        {
            Console.Write(message);
            return double.Parse(Console.ReadLine() ?? "0");
        }

        private int ReadInteger(string message)
        {
            Console.Write(message);
            return int.Parse(Console.ReadLine() ?? "0");
        }

        private LengthEnum ReadLengthUnit()
        {
            Console.WriteLine("1. FEET  2. INCHES  3. YARDS  4. CENTIMETERS");
            return (LengthEnum)(ReadInteger("Choose unit: ") - 1);
        }

        private WeightEnum ReadWeightUnit()
        {
            Console.WriteLine("1. KILOGRAM  2. GRAM  3. POUND");
            return (WeightEnum)(ReadInteger("Choose unit: ") - 1);
        }

        private VolumeEnum ReadVolumeUnit()
        {
            Console.WriteLine("1. LITRE  2. MILLILITRE  3. GALLON");
            return (VolumeEnum)(ReadInteger("Choose unit: ") - 1);
        }
    }
}