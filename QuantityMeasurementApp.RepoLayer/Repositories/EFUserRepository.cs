using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using QuantityMeasurementApp.ModelLayer.Entities;
using QuantityMeasurementApp.RepoLayer.Data;
using QuantityMeasurementApp.RepoLayer.Interfaces;

namespace QuantityMeasurementApp.RepoLayer.Repositories
{
    /// <summary>
    /// EF Core implementation of user repository.
    /// Saves users to SQL Server — data visible in SSMS.
    /// </summary>
    public class EFUserRepository : IUserRepository
    {
        private readonly AppDbContext          _context;
        private readonly ILogger<EFUserRepository> _logger;

        public EFUserRepository(AppDbContext context, ILogger<EFUserRepository> logger)
        {
            _context = context;
            _logger  = logger;
        }

        /// <summary>Saves a new user to SQL Server.</summary>
        public void Save(UserEntity user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            _logger.LogInformation("User saved to DB: {Email}", user.Email);
        }

        /// <summary>Finds user by email using LINQ.</summary>
        public UserEntity FindByEmail(string email)
            => _context.Users
                       .FirstOrDefault(u => u.Email == email.ToLowerInvariant());

        /// <summary>Checks if email already exists using LINQ.</summary>
        public bool ExistsByEmail(string email)
            => _context.Users
                       .Any(u => u.Email == email.ToLowerInvariant());

        /// <summary>Updates last login timestamp in SQL Server.</summary>
        public void UpdateLastLogin(string email)
        {
            var user = _context.Users
                               .FirstOrDefault(u => u.Email == email.ToLowerInvariant());
            if (user != null)
            {
                user.LastLoginAt = DateTime.UtcNow;
                _context.SaveChanges();
                _logger.LogInformation("LastLogin updated for: {Email}", email);
            }
        }
    }
}
