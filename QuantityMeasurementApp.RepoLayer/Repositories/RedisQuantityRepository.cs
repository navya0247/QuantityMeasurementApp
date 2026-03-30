using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using QuantityMeasurementApp.ModelLayer.Entities;
using QuantityMeasurementApp.RepoLayer.Interfaces;
using StackExchange.Redis;

namespace QuantityMeasurementApp.RepoLayer.Repositories
{
    /// <summary>
    /// Redis cache repository — stores quantity measurements in Redis.
    /// Implements IRedisQuantityRepository which extends IQuantityMeasurementRepository.
    /// </summary>
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

        /// <summary>Saves entity to Redis cache.</summary>
        public void Save(QuantityMeasurementEntity entity)
        {
            var all = GetAllFromCache();
            all.Add(entity);
            SaveAllToCache(all);
        }

        /// <summary>Returns all records ordered by most recent first.</summary>
        public List<QuantityMeasurementEntity> GetAll()
            => GetAllFromCache()
                .OrderByDescending(q => q.CreatedAt)
                .ToList();

        /// <summary>Filters by operation type using LINQ.</summary>
        public List<QuantityMeasurementEntity> GetByOperation(string operation)
            => GetAllFromCache()
                .Where(q => q.Operation == operation.ToUpper())
                .OrderByDescending(q => q.CreatedAt)
                .ToList();

        /// <summary>Filters by measurement type using LINQ.</summary>
        public List<QuantityMeasurementEntity> GetByMeasureType(string measureType)
            => GetAllFromCache()
                .Where(q => q.MeasureType == measureType.ToUpper())
                .OrderByDescending(q => q.CreatedAt)
                .ToList();

        /// <summary>Returns all error records.</summary>
        public List<QuantityMeasurementEntity> GetErrored()
            => GetAllFromCache()
                .Where(q => q.IsError)
                .OrderByDescending(q => q.CreatedAt)
                .ToList();

        /// <summary>Returns total count of all records in cache.</summary>
        public int GetTotalCount() => GetAllFromCache().Count;

        /// <summary>Clears all records from Redis cache.</summary>
        public void DeleteAll() => _cache.KeyDelete(AllKey);

        //  Private helpers 

        private List<QuantityMeasurementEntity> GetAllFromCache()
        {
            var json = _cache.StringGet(AllKey);
            if (json.IsNullOrEmpty) return new List<QuantityMeasurementEntity>();
            return JsonSerializer.Deserialize<List<QuantityMeasurementEntity>>(json)
                   ?? new List<QuantityMeasurementEntity>();
        }

        private void SaveAllToCache(List<QuantityMeasurementEntity> entities)
        {
            var json = JsonSerializer.Serialize(entities);
            _cache.StringSet(AllKey, json, TimeSpan.FromHours(TtlHours));
        }
    }
}
