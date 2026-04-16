using System;
using AuthService.Data;
using AuthService.Entities;
using AuthService.Interfaces;

namespace AuthService.Repositories
{
    public class EFUserRepository : IUserRepository
    {
        private readonly AuthDbContext _db;

        public EFUserRepository(AuthDbContext db) => _db = db;

        public void Save(UserEntity user)
        {
            _db.Users.Add(user);
            _db.SaveChanges();
        }

        public UserEntity? FindByEmail(string email)
            => _db.Users.FirstOrDefault(u => u.Email == email.ToLowerInvariant());

        public bool ExistsByEmail(string email)
            => _db.Users.Any(u => u.Email == email.ToLowerInvariant());

        public void UpdateLastLogin(string email)
        {
            var user = FindByEmail(email);
            if (user == null) return;
            user.LastLoginAt = DateTime.UtcNow;
            _db.SaveChanges();
        }
    }
}
