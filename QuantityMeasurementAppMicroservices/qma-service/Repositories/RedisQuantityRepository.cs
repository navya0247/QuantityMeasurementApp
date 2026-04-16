using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using QmaService.Entities;
using QmaService.Interfaces;
using StackExchange.Redis;

namespace QmaService.Repositories
{
    public class RedisQuantityRepository : IRedisQuantityRepository
    {
        private readonly IDatabase              _cache;
        private readonly IConnectionMultiplexer _redis;
        private const string                    AllKey   = "measurements:all";
        private const int                       TtlHours = 24;

        public RedisQuantityRepository(IConnectionMultiplexer redis)
        {
            _redis = redis;
            _cache = redis.GetDatabase();
        }

        public void Save(QuantityMeasurementEntity entity)
        {
            var all = GetAllFromCache();
            all.Add(entity);
            SaveAllToCache(all);
        }

        public List<QuantityMeasurementEntity> GetAll()
            => GetAllFromCache().OrderByDescending(q => q.CreatedAt).ToList();

        public List<QuantityMeasurementEntity> GetByOperation(string operation)
            => GetAllFromCache()
                .Where(q => q.Operation == operation.ToUpper())
                .OrderByDescending(q => q.CreatedAt).ToList();

        public List<QuantityMeasurementEntity> GetByMeasureType(string measureType)
            => GetAllFromCache()
                .Where(q => q.MeasureType == measureType.ToUpper())
                .OrderByDescending(q => q.CreatedAt).ToList();

        public List<QuantityMeasurementEntity> GetErrored()
            => GetAllFromCache()
                .Where(q => q.IsError)
                .OrderByDescending(q => q.CreatedAt).ToList();

        public int  GetTotalCount() => GetAllFromCache().Count;
        public void DeleteAll()     => _cache.KeyDelete(AllKey);

        private List<QuantityMeasurementEntity> GetAllFromCache()
        {
            var json = _cache.StringGet(AllKey);
            if (json.IsNullOrEmpty) return new List<QuantityMeasurementEntity>();
            return JsonSerializer.Deserialize<List<QuantityMeasurementEntity>>(json!)
                   ?? new List<QuantityMeasurementEntity>();
        }

        private void SaveAllToCache(List<QuantityMeasurementEntity> entities)
        {
            var json = JsonSerializer.Serialize(entities);
            _cache.StringSet(AllKey, json, TimeSpan.FromHours(TtlHours));
        }
    }
}
