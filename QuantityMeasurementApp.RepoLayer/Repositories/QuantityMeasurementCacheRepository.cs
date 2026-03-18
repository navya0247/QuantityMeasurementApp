using QuantityMeasurementApp.ModelLayer.Entities;
using QuantityMeasurementApp.RepoLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace QuantityMeasurementApp.RepoLayer.Repositories
{
    public class QuantityMeasurementCacheRepository : IQuantityMeasurementRepository
    {
        private static QuantityMeasurementCacheRepository _instance;
        private readonly List<QuantityMeasurementEntity> _cache = new();
        private readonly string _jsonFilePath = "quantity_measurements.json";

        private QuantityMeasurementCacheRepository()
        {
            LoadFromJson();
        }

        public static QuantityMeasurementCacheRepository GetInstance()
        {
            _instance ??= new QuantityMeasurementCacheRepository();
            return _instance;
        }

        public void Save(QuantityMeasurementEntity entity)
        {
            // Save to memory
            _cache.Add(entity);

            // Save to JSON file
            SaveToJson(entity);
        }

        // ── JSON Operations ───────────────────────────────────────────────

        private void SaveToJson(QuantityMeasurementEntity entity)
        {
            try
            {
                List<JsonEntityModel> existing = new();

                if (File.Exists(_jsonFilePath))
                {
                    string existingJson = File.ReadAllText(_jsonFilePath);
                    if (!string.IsNullOrWhiteSpace(existingJson))
                    {
                        existing = JsonSerializer.Deserialize<List<JsonEntityModel>>(
                            existingJson) ?? new List<JsonEntityModel>();
                    }
                }

                existing.Add(new JsonEntityModel
                {
                    Id          = entity.Id.ToString(),
                    Operation   = entity.Operation,
                    OperandOne  = entity.OperandOne,
                    OperandTwo  = entity.OperandTwo,
                    Result      = entity.Result,
                    MeasureType = entity.MeasureType,
                    CreatedAt   = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                });

                string json = JsonSerializer.Serialize(existing,
                    new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_jsonFilePath, json);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"Warning: Could not save to JSON. {ex.Message}");
            }
        }

        private void LoadFromJson()
        {
            try
            {
                if (!File.Exists(_jsonFilePath)) return;

                string json = File.ReadAllText(_jsonFilePath);
                if (string.IsNullOrWhiteSpace(json)) return;

                var records = JsonSerializer.Deserialize<List<JsonEntityModel>>(json);
                if (records == null) return;

                foreach (var r in records)
                {
                    _cache.Add(new QuantityMeasurementEntity(
                        r.Operation,
                        r.OperandOne,
                        r.OperandTwo,
                        r.Result,
                        r.MeasureType)
                    { Id = Guid.Parse(r.Id) });
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"Warning: Could not load from JSON. {ex.Message}");
            }
        }

        // ── Repository Methods ────────────────────────────────────────────

        public List<QuantityMeasurementEntity> GetAll()
        {
            List<QuantityMeasurementEntity> result = new();
            foreach (var entity in _cache)
                result.Add(entity);
            return result;
        }

        public List<QuantityMeasurementEntity> GetByOperation(string operation)
        {
            List<QuantityMeasurementEntity> result = new();
            foreach (var entity in _cache)
            {
                if (entity.Operation == operation.ToUpper())
                    result.Add(entity);
            }
            return result;
        }

        public List<QuantityMeasurementEntity> GetByMeasureType(string measureType)
        {
            List<QuantityMeasurementEntity> result = new();
            foreach (var entity in _cache)
            {
                if (entity.MeasureType == measureType.ToUpper())
                    result.Add(entity);
            }
            return result;
        }

        public List<QuantityMeasurementEntity> GetFullHistory()
        {
            List<QuantityMeasurementEntity> result = new();
            foreach (var entity in _cache)
                result.Add(entity);
            return result;
        }

        public int GetTotalCount()
        {
            return _cache.Count;
        }

        public void DeleteAll()
        {
            _cache.Clear();
            try
            {
                if (File.Exists(_jsonFilePath))
                    File.WriteAllText(_jsonFilePath, "[]");
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"Warning: Could not clear JSON. {ex.Message}");
            }
        }

        public string GetPoolStats()
        {
            return $"Cache Repository | Total records: {_cache.Count} | " +
                   $"JSON file: {_jsonFilePath}";
        }

        // ── JSON Model ────────────────────────────────────────────────────

        private class JsonEntityModel
        {
            public string Id          { get; set; }
            public string Operation   { get; set; }
            public string OperandOne  { get; set; }
            public string OperandTwo  { get; set; }
            public string Result      { get; set; }
            public string MeasureType { get; set; }
            public string CreatedAt   { get; set; }
        }
    }
}