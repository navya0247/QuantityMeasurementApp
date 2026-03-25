using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using QuantityMeasurementApp.BusinessLayer.Interfaces;
using QuantityMeasurementApp.ModelLayer.DTO;
using QuantityMeasurementApp.ModelLayer.Entities;
using QuantityMeasurementApp.RepoLayer.Interfaces;

namespace QuantityMeasurementApp.BusinessLayer.Services
{
    /// <summary>
    /// Handles user registration and login.
    ///
    /// Security chain on signup:
    ///   1. BCrypt hashes the password with work factor 12
    ///   2. BCrypt internally generates and embeds a unique random salt
    ///   3. Hash stored in database — plain password never saved
    ///
    /// Security chain on signin:
    ///   1. Load user by email from database
    ///   2. BCrypt.Verify(enteredPassword, storedHash)
    ///      — BCrypt extracts embedded salt automatically
    ///   3. If valid → generate and return JWT token
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly IUserRepository      _users;
        private readonly IJwtTokenService     _jwt;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            IUserRepository      users,
            IJwtTokenService     jwt,
            ILogger<AuthService> logger)
        {
            _users  = users;
            _jwt    = jwt;
            _logger = logger;
        }

        /// <summary>
        /// Registers a new user.
        /// BCrypt hashes the password with work factor 12 and embeds
        /// a unique random salt automatically inside the hash string.
        /// No separate salt column needed — BCrypt handles it internally.
        /// </summary>
        public UserProfileDTO SignUp(SignUpDTO dto)
        {
            _logger.LogInformation("SignUp attempt for email: {Email}", dto.Email);

            if (_users.ExistsByEmail(dto.Email))
            {
                _logger.LogWarning("SignUp failed — email already exists: {Email}", dto.Email);
                throw new InvalidOperationException(
                    $"Account with email '{dto.Email}' already exists.");
            }

            var user = new UserEntity
            {
                FullName     = dto.FullName.Trim(),
                Email        = dto.Email.Trim().ToLowerInvariant(),
                // BCrypt generates unique random salt internally per user
                // and embeds it in the hash — no separate salt storage needed
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password, workFactor: 12),
                Role         = "User"
            };

            _users.Save(user);
            _logger.LogInformation("User registered successfully: {Email}", user.Email);
            return ToProfile(user);
        }

        /// <summary>
        /// Validates credentials and returns a signed JWT token.
        /// BCrypt.Verify extracts the embedded salt from the stored hash
        /// and compares it with the entered password automatically.
        /// </summary>
        public AuthResponseDTO SignIn(SignInDTO dto)
        {
            _logger.LogInformation("SignIn attempt for email: {Email}", dto.Email);

            string email = dto.Email.Trim().ToLowerInvariant();
            var user     = _users.FindByEmail(email);

            // BCrypt.Verify handles salt extraction internally from stored hash
            if (user == null || !user.IsActive ||
                !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            {
                _logger.LogWarning("SignIn failed for: {Email}", dto.Email);
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            _users.UpdateLastLogin(email);
            _logger.LogInformation("SignIn successful: {Email}", email);

            return new AuthResponseDTO
            {
                Token            = _jwt.GenerateToken(user),
                ExpiresInSeconds = _jwt.GetExpirySeconds(),
                Email            = user.Email,
                FullName         = user.FullName,
                Role             = user.Role
            };
        }

        /// <summary>Returns profile of the currently authenticated user.</summary>
        public UserProfileDTO GetProfile(string email)
        {
            _logger.LogInformation("GetProfile for: {Email}", email);
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