using System;
using System.Collections.Concurrent;
using QuantityMeasurementApp.ModelLayer.Entities;
using QuantityMeasurementApp.RepoLayer.Interfaces;

namespace QuantityMeasurementApp.RepoLayer.Repositories
{
    /// <summary>Thread-safe in-memory user store using ConcurrentDictionary.</summary>
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly ConcurrentDictionary<string, UserEntity> _store
            = new ConcurrentDictionary<string, UserEntity>(StringComparer.OrdinalIgnoreCase);

        public void Save(UserEntity user) => _store[user.Email] = user;

        public UserEntity FindByEmail(string email)
        {
            _store.TryGetValue(email, out var user);
            return user;
        }

        public bool ExistsByEmail(string email) => _store.ContainsKey(email);

        public void UpdateLastLogin(string email)
        {
            if (_store.TryGetValue(email, out var user))
                user.LastLoginAt = DateTime.UtcNow;
        }
    }
}
