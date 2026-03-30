using Microsoft.IdentityModel.Tokens;
using QuantityMeasurementApp.ModelLayer.Entities;

namespace QuantityMeasurementApp.BusinessLayer.Interfaces
{
    /// <summary>Contract for JWT token generation and validation.</summary>
    public interface IJwtTokenService
    {
        string GenerateToken(UserEntity user);
        int GetExpirySeconds();
        TokenValidationParameters GetValidationParameters();
    }
}
