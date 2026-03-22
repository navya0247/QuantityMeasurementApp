using QuantityMeasurementApp.BusinessLayer.Interfaces;
using QuantityMeasurementApp.BusinessLayer.Services;
using QuantityMeasurementApp.ModelLayer.DTO;
using QuantityMeasurementApp.ModelLayer.Models;
using QuantityMeasurementApp.RepoLayer.Interfaces;
using QuantityMeasurementApp.RepoLayer.Repositories;
using System;

namespace QuantityMeasurementApp.Controller
{
    /// <summary>Console controller — handles user input and displays results.</summary>
    public class QuantityMeasurementController
    {
        private readonly IQuantityMeasurementService _service;

        public QuantityMeasurementController()
        {
            // Uses EF Core repository with in-memory SQLite for console app
            IQuantityMeasurementRepository repository = new InMemoryQuantityRepository();
            _service = new QuantityMeasurementServiceImpl(repository);
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
                    case 1: ShowLengthOperations();      break;
                    case 2: ShowWeightOperations();      break;
                    case 3: ShowVolumeOperations();      break;
                    case 4: ShowTemperatureOperations(); break;
                    case 5: ShowHistoryMenu();           break;
                    case 6: isRunning = false;           break;
                    default: Console.WriteLine("Invalid choice."); break;
                }
            }
            Console.WriteLine("\nGoodbye!");
        }

        // ── History ───────────────────────────────────────────────────────

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
            var records = _service.GetAllMeasurements();
            Console.WriteLine($"\n--- All History ({records.Count} records) ---");
            foreach (var r in records)
                Console.WriteLine($"[{r.MeasureType}] {r.Operation}: {r.OperandOne} | {r.OperandTwo} => {r.Result}");
        }

        private void ViewByOperation()
        {
            Console.Write("Enter operation (COMPARE/ADD/SUBTRACT/DIVIDE/CONVERT): ");
            string op      = Console.ReadLine()?.Trim().ToUpper() ?? "";
            var records    = _service.GetByOperation(op);
            Console.WriteLine($"\n--- {op} ({records.Count} records) ---");
            foreach (var r in records)
                Console.WriteLine($"[{r.MeasureType}] {r.OperandOne} | {r.OperandTwo} => {r.Result}");
        }

        private void ViewByMeasurementType()
        {
            Console.Write("Enter type (LENGTH/WEIGHT/VOLUME/TEMPERATURE): ");
            string mt   = Console.ReadLine()?.Trim().ToUpper() ?? "";
            var records = _service.GetByMeasureType(mt);
            Console.WriteLine($"\n--- {mt} ({records.Count} records) ---");
            foreach (var r in records)
                Console.WriteLine($"[{r.Operation}] {r.OperandOne} | {r.OperandTwo} => {r.Result}");
        }

        private void ViewStatistics()
        {
            Console.WriteLine("\n--- Statistics ---");
            Console.WriteLine($"Total records: {_service.GetTotalCount()}");
        }

        private void ClearAllRecords()
        {
            Console.Write("Are you sure? (yes/no): ");
            if (Console.ReadLine()?.Trim().ToLower() == "yes")
            {
                _service.DeleteAll();
                Console.WriteLine("All records cleared.");
            }
        }

        // ── Length ────────────────────────────────────────────────────────

        private void ShowLengthOperations()
        {
            Console.WriteLine("\n===== Length Operations =====");
            Console.WriteLine("1. Compare  2. Convert  3. Add  4. Subtract  5. Divide");
            int option = ReadInteger("Select: ");
            switch (option)
            {
                case 1: CompareLength();  break;
                case 2: ConvertLength();  break;
                case 3: AddLength();      break;
                case 4: SubtractLength(); break;
                case 5: DivideLength();   break;
                default: Console.WriteLine("Invalid."); break;
            }
        }

        private void CompareLength()
        {
            double v1 = ReadDouble("First length: ");   LengthEnum u1 = ReadLengthUnit();
            double v2 = ReadDouble("Second length: ");  LengthEnum u2 = ReadLengthUnit();
            var dto   = _service.Compare(new QuantityDTO(v1, u1), new QuantityDTO(v2, u2));
            Console.WriteLine("Equal: " + dto.Result);
        }

        private void ConvertLength()
        {
            double v = ReadDouble("Length value: ");
            Console.WriteLine("From:"); LengthEnum from = ReadLengthUnit();
            Console.WriteLine("To:");   LengthEnum to   = ReadLengthUnit();
            var dto  = _service.Convert(new QuantityDTO(v, from), to);
            Console.WriteLine("Result: " + dto.Result);
        }

        private void AddLength()
        {
            double v1 = ReadDouble("First: ");  LengthEnum u1 = ReadLengthUnit();
            double v2 = ReadDouble("Second: "); LengthEnum u2 = ReadLengthUnit();
            Console.WriteLine("Target unit:"); LengthEnum t = ReadLengthUnit();
            var dto   = _service.Add(new QuantityDTO(v1, u1), new QuantityDTO(v2, u2), t);
            Console.WriteLine("Result: " + dto.Result);
        }

        private void SubtractLength()
        {
            double v1 = ReadDouble("First: ");  LengthEnum u1 = ReadLengthUnit();
            double v2 = ReadDouble("Second: "); LengthEnum u2 = ReadLengthUnit();
            var dto   = _service.Subtract(new QuantityDTO(v1, u1), new QuantityDTO(v2, u2));
            Console.WriteLine("Result: " + dto.Result);
        }

        private void DivideLength()
        {
            double v1 = ReadDouble("First: ");  LengthEnum u1 = ReadLengthUnit();
            double v2 = ReadDouble("Second: "); LengthEnum u2 = ReadLengthUnit();
            var dto   = _service.Divide(new QuantityDTO(v1, u1), new QuantityDTO(v2, u2));
            Console.WriteLine("Result: " + dto.Result);
        }

        // ── Weight ────────────────────────────────────────────────────────

        private void ShowWeightOperations()
        {
            Console.WriteLine("\n===== Weight Operations =====");
            Console.WriteLine("1. Compare  2. Convert  3. Add  4. Subtract  5. Divide");
            int option = ReadInteger("Select: ");
            switch (option)
            {
                case 1: CompareWeight();  break;
                case 2: ConvertWeight();  break;
                case 3: AddWeight();      break;
                case 4: SubtractWeight(); break;
                case 5: DivideWeight();   break;
                default: Console.WriteLine("Invalid."); break;
            }
        }

        private void CompareWeight()
        {
            double v1 = ReadDouble("First: ");  WeightEnum u1 = ReadWeightUnit();
            double v2 = ReadDouble("Second: "); WeightEnum u2 = ReadWeightUnit();
            var dto   = _service.Compare(new QuantityDTO(v1, u1), new QuantityDTO(v2, u2));
            Console.WriteLine("Equal: " + dto.Result);
        }

        private void ConvertWeight()
        {
            double v = ReadDouble("Weight value: ");
            Console.WriteLine("From:"); WeightEnum from = ReadWeightUnit();
            Console.WriteLine("To:");   WeightEnum to   = ReadWeightUnit();
            var dto  = _service.Convert(new QuantityDTO(v, from), to);
            Console.WriteLine("Result: " + dto.Result);
        }

        private void AddWeight()
        {
            double v1 = ReadDouble("First: ");  WeightEnum u1 = ReadWeightUnit();
            double v2 = ReadDouble("Second: "); WeightEnum u2 = ReadWeightUnit();
            Console.WriteLine("Target:"); WeightEnum t = ReadWeightUnit();
            var dto   = _service.Add(new QuantityDTO(v1, u1), new QuantityDTO(v2, u2), t);
            Console.WriteLine("Result: " + dto.Result);
        }

        private void SubtractWeight()
        {
            double v1 = ReadDouble("First: ");  WeightEnum u1 = ReadWeightUnit();
            double v2 = ReadDouble("Second: "); WeightEnum u2 = ReadWeightUnit();
            var dto   = _service.Subtract(new QuantityDTO(v1, u1), new QuantityDTO(v2, u2));
            Console.WriteLine("Result: " + dto.Result);
        }

        private void DivideWeight()
        {
            double v1 = ReadDouble("First: ");  WeightEnum u1 = ReadWeightUnit();
            double v2 = ReadDouble("Second: "); WeightEnum u2 = ReadWeightUnit();
            var dto   = _service.Divide(new QuantityDTO(v1, u1), new QuantityDTO(v2, u2));
            Console.WriteLine("Result: " + dto.Result);
        }

        // ── Volume ────────────────────────────────────────────────────────

        private void ShowVolumeOperations()
        {
            Console.WriteLine("\n===== Volume Operations =====");
            Console.WriteLine("1. Compare  2. Convert  3. Add  4. Subtract  5. Divide");
            int option = ReadInteger("Select: ");
            switch (option)
            {
                case 1: CompareVolume();  break;
                case 2: ConvertVolume();  break;
                case 3: AddVolume();      break;
                case 4: SubtractVolume(); break;
                case 5: DivideVolume();   break;
                default: Console.WriteLine("Invalid."); break;
            }
        }

        private void CompareVolume()
        {
            double v1 = ReadDouble("First: ");  VolumeEnum u1 = ReadVolumeUnit();
            double v2 = ReadDouble("Second: "); VolumeEnum u2 = ReadVolumeUnit();
            var dto   = _service.Compare(new QuantityDTO(v1, u1), new QuantityDTO(v2, u2));
            Console.WriteLine("Equal: " + dto.Result);
        }

        private void ConvertVolume()
        {
            double v = ReadDouble("Volume value: ");
            Console.WriteLine("From:"); VolumeEnum from = ReadVolumeUnit();
            Console.WriteLine("To:");   VolumeEnum to   = ReadVolumeUnit();
            var dto  = _service.Convert(new QuantityDTO(v, from), to);
            Console.WriteLine("Result: " + dto.Result);
        }

        private void AddVolume()
        {
            double v1 = ReadDouble("First: ");  VolumeEnum u1 = ReadVolumeUnit();
            double v2 = ReadDouble("Second: "); VolumeEnum u2 = ReadVolumeUnit();
            Console.WriteLine("Target:"); VolumeEnum t = ReadVolumeUnit();
            var dto   = _service.Add(new QuantityDTO(v1, u1), new QuantityDTO(v2, u2), t);
            Console.WriteLine("Result: " + dto.Result);
        }

        private void SubtractVolume()
        {
            double v1 = ReadDouble("First: ");  VolumeEnum u1 = ReadVolumeUnit();
            double v2 = ReadDouble("Second: "); VolumeEnum u2 = ReadVolumeUnit();
            var dto   = _service.Subtract(new QuantityDTO(v1, u1), new QuantityDTO(v2, u2));
            Console.WriteLine("Result: " + dto.Result);
        }

        private void DivideVolume()
        {
            double v1 = ReadDouble("First: ");  VolumeEnum u1 = ReadVolumeUnit();
            double v2 = ReadDouble("Second: "); VolumeEnum u2 = ReadVolumeUnit();
            var dto   = _service.Divide(new QuantityDTO(v1, u1), new QuantityDTO(v2, u2));
            Console.WriteLine("Result: " + dto.Result);
        }

        // ── Temperature ───────────────────────────────────────────────────

        private void ShowTemperatureOperations()
        {
            Console.WriteLine("\n===== Temperature Operations =====");
            Console.WriteLine("1. Compare  2. Convert");
            int option = ReadInteger("Select: ");
            switch (option)
            {
                case 1: CompareTemperature(); break;
                case 2: ConvertTemperature(); break;
                default: Console.WriteLine("Invalid."); break;
            }
        }

        private void CompareTemperature()
        {
            double v1  = ReadDouble("First temperature: ");
            double v2  = ReadDouble("Second temperature: ");
            var first  = new Quantity<TemperatureEnum>(v1, TemperatureEnum.CELSIUS);
            var second = new Quantity<TemperatureEnum>(v2, TemperatureEnum.FAHRENHEIT);
            Console.WriteLine("Equal: " + first.Equals(second));
        }

        private void ConvertTemperature()
        {
            double v = ReadDouble("Temperature value: ");
            Console.WriteLine("From:"); TemperatureEnum from = ReadTemperatureUnit();
            Console.WriteLine("To:");   TemperatureEnum to   = ReadTemperatureUnit();
            var dto  = _service.Convert(new QuantityDTO(v, from), to);
            Console.WriteLine("Result: " + dto.Result);
        }

        // ── Helpers ───────────────────────────────────────────────────────

        private double ReadDouble(string msg)
        {
            Console.Write(msg);
            return double.Parse(Console.ReadLine() ?? "0");
        }

        private int ReadInteger(string msg)
        {
            Console.Write(msg);
            return int.Parse(Console.ReadLine() ?? "0");
        }

        private LengthEnum ReadLengthUnit()
        {
            Console.WriteLine("1.FEET 2.INCHES 3.YARDS 4.CENTIMETERS");
            return (LengthEnum)(ReadInteger("Unit: ") - 1);
        }

        private WeightEnum ReadWeightUnit()
        {
            Console.WriteLine("1.KILOGRAM 2.GRAM 3.POUND");
            return (WeightEnum)(ReadInteger("Unit: ") - 1);
        }

        private VolumeEnum ReadVolumeUnit()
        {
            Console.WriteLine("1.LITRE 2.MILLILITRE 3.GALLON");
            return (VolumeEnum)(ReadInteger("Unit: ") - 1);
        }

        private TemperatureEnum ReadTemperatureUnit()
        {
            Console.WriteLine("1.CELSIUS 2.FAHRENHEIT 3.KELVIN");
            return (TemperatureEnum)(ReadInteger("Unit: ") - 1);
        }
    }
}
