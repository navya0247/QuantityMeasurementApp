using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using AuthService.DTO;
using AuthService.Entities;
using AuthService.Interfaces;

namespace AuthService.Services
{
    public class AuthServiceImpl : IAuthService
    {
        private readonly IUserRepository      _users;
        private readonly IJwtTokenService     _jwt;
        private readonly ILogger<AuthServiceImpl> _logger;

        public AuthServiceImpl(
            IUserRepository         users,
            IJwtTokenService        jwt,
            ILogger<AuthServiceImpl> logger)
        {
            _users  = users;
            _jwt    = jwt;
            _logger = logger;
        }

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
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password, workFactor: 12),
                Role         = "User"
            };

            _users.Save(user);
            _logger.LogInformation("User registered successfully: {Email}", user.Email);
            return ToProfile(user);
        }

        public AuthResponseDTO SignIn(SignInDTO dto)
        {
            _logger.LogInformation("SignIn attempt for email: {Email}", dto.Email);

            string email = dto.Email.Trim().ToLowerInvariant();
            var user     = _users.FindByEmail(email);

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
