using System;
using System.Collections.Generic;
using QuantityMeasurementApp.BusinessLayer.Interfaces;
using QuantityMeasurementApp.ModelLayer.DTO;
using QuantityMeasurementApp.ModelLayer.Entities;
using QuantityMeasurementApp.RepoLayer.Interfaces;

namespace QuantityMeasurementApp.BusinessLayer.Services
{
    /// <summary>Handles user registration and login with BCrypt password hashing.</summary>
    public class AuthService : IAuthService
    {
        private readonly IUserRepository  _users;
        private readonly IJwtTokenService _jwt;

        public AuthService(IUserRepository users, IJwtTokenService jwt)
        {
            _users = users;
            _jwt   = jwt;
        }

        /// <summary>Registers a new user after checking for duplicate email.</summary>
        public UserProfileDTO SignUp(SignUpDTO dto)
        {
            if (_users.ExistsByEmail(dto.Email))
                throw new InvalidOperationException(
                    $"Account with email '{dto.Email}' already exists.");

            var user = new UserEntity
            {
                FullName     = dto.FullName.Trim(),
                Email        = dto.Email.Trim().ToLowerInvariant(),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password, workFactor: 12),
                Role         = "User"
            };
            _users.Save(user);
            return ToProfile(user);
        }

        /// <summary>Validates credentials and returns a JWT token response.</summary>
        public AuthResponseDTO SignIn(SignInDTO dto)
        {
            string email = dto.Email.Trim().ToLowerInvariant();
            var user     = _users.FindByEmail(email);

            // Generic message prevents user-enumeration attacks
            if (user == null || !user.IsActive ||
                !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid email or password.");

            _users.UpdateLastLogin(email);

            return new AuthResponseDTO
            {
                Token            = _jwt.GenerateToken(user),
                ExpiresInSeconds = _jwt.GetExpirySeconds(),
                Email            = user.Email,
                FullName         = user.FullName,
                Role             = user.Role
            };
        }

        /// <summary>Returns profile of user identified by email.</summary>
        public UserProfileDTO GetProfile(string email)
        {
            var user = _users.FindByEmail(email)
                ?? throw new KeyNotFoundException($"User '{email}' not found.");
            return ToProfile(user);
        }

        private static UserProfileDTO ToProfile(UserEntity u) => new UserProfileDTO
        {
            Id          = u.Id.ToString(),
            FullName    = u.FullName,
            Email       = u.Email,
            Role        = u.Role,
            CreatedAt   = u.CreatedAt.ToString("o"),
            LastLoginAt = u.LastLoginAt?.ToString("o")
        };
    }
}
