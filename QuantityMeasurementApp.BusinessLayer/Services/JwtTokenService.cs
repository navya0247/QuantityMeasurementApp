using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using QuantityMeasurementApp.BusinessLayer.Interfaces;
using QuantityMeasurementApp.ModelLayer.Entities;

namespace QuantityMeasurementApp.BusinessLayer.Services
{
    /// <summary>Generates and validates JWT tokens </summary>
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _config;

        public JwtTokenService(IConfiguration config) => _config = config;

        /// <summary>Creates a signed JWT token for the given user.</summary>
        public string GenerateToken(UserEntity user)
        {
            var key    = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GetKey()));
            var creds  = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiry = DateTime.UtcNow.AddHours(GetExpiryHours());

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub,   user.Email),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti,   Guid.NewGuid().ToString()),
                new Claim("fullName",                    user.FullName),
                new Claim(ClaimTypes.Role,               user.Role)
            };

            var token = new JwtSecurityToken(
                issuer:             GetIssuer(),
                audience:           GetAudience(),
                claims:             claims,
                expires:            expiry,
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>Returns token expiry in seconds.</summary>
        public int GetExpirySeconds() => GetExpiryHours() * 3600;

        /// <summary>Returns parameters used to validate incoming tokens.</summary>
        public TokenValidationParameters GetValidationParameters() =>
            new TokenValidationParameters
            {
                ValidateIssuer           = true,
                ValidateAudience         = true,
                ValidateLifetime         = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer              = GetIssuer(),
                ValidAudience            = GetAudience(),
                IssuerSigningKey         = new SymmetricSecurityKey(
                                               Encoding.UTF8.GetBytes(GetKey()))
            };

        private string GetKey()
            => _config["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not configured.");
        private string GetIssuer()      => _config["Jwt:Issuer"]   ?? "QuantityMeasurementApp";
        private string GetAudience()    => _config["Jwt:Audience"] ?? "QuantityMeasurementApp";
        private int    GetExpiryHours() => int.TryParse(_config["Jwt:ExpiryHours"], out int h) ? h : 24;
    }
}
