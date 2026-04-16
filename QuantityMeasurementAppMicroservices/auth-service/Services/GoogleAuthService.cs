using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using AuthService.DTO;
using AuthService.Entities;
using AuthService.Interfaces;

namespace AuthService.Services
{
    /// <summary>
    /// Validates Google ID tokens, auto-creates or retrieves user, issues JWT.
    /// </summary>
    public class GoogleAuthService : IGoogleAuthService
    {
        private readonly IUserRepository            _users;
        private readonly IJwtTokenService           _jwt;
        private readonly IConfiguration             _config;
        private readonly ILogger<GoogleAuthService> _logger;

        public GoogleAuthService(
            IUserRepository            users,
            IJwtTokenService           jwt,
            IConfiguration             config,
            ILogger<GoogleAuthService> logger)
        {
            _users  = users;
            _jwt    = jwt;
            _config = config;
            _logger = logger;
        }

        public async Task<AuthResponseDTO> SignInWithGoogleAsync(string idToken)
        {
            var googleClientId = _config["Google:ClientId"]
                ?? throw new InvalidOperationException("Google:ClientId missing from appsettings.json");

            GoogleJsonWebSignature.Payload payload;
            try
            {
                payload = await GoogleJsonWebSignature.ValidateAsync(idToken,
                    new GoogleJsonWebSignature.ValidationSettings
                    {
                        Audience = new[] { googleClientId }
                    });
            }
            catch (InvalidJwtException ex)
            {
                _logger.LogWarning("Google token validation failed: {Message}", ex.Message);
                throw new UnauthorizedAccessException("Invalid Google ID token.");
            }

            _logger.LogInformation("Google sign-in verified for {Email}", payload.Email);

            var user = _users.FindByEmail(payload.Email);

            if (user == null)
            {
                user = new UserEntity
                {
                    FullName     = payload.Name ?? payload.Email.Split('@')[0],
                    Email        = payload.Email.ToLowerInvariant(),
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(Guid.NewGuid().ToString()),
                    Role         = "User",
                    AuthProvider = "Google",
                    IsActive     = true,
                    CreatedAt    = DateTime.UtcNow
                };
                _users.Save(user);
                _logger.LogInformation("Auto-registered Google user: {Email}", payload.Email);
            }
            else if (user.AuthProvider != "Google")
            {
                user.AuthProvider = "Google";
                _users.UpdateLastLogin(user.Email);
            }

            return new AuthResponseDTO
            {
                Token            = _jwt.GenerateToken(user),
                ExpiresInSeconds = _jwt.GetExpirySeconds(),
                Email            = user.Email,
                FullName         = user.FullName,
                Role             = user.Role
            };
        }
    }
}
