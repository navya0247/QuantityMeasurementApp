using AuthService.DTO;
using AuthService.Entities;
using Microsoft.IdentityModel.Tokens;

namespace AuthService.Interfaces
{
    public interface IAuthService
    {
        UserProfileDTO  SignUp(SignUpDTO dto);
        AuthResponseDTO SignIn(SignInDTO dto);
        UserProfileDTO  GetProfile(string email);
    }

    public interface IGoogleAuthService
    {
        Task<AuthResponseDTO> SignInWithGoogleAsync(string idToken);
    }

    public interface IJwtTokenService
    {
        string                    GenerateToken(UserEntity user);
        int                       GetExpirySeconds();
        TokenValidationParameters GetValidationParameters();
    }

    public interface IEncryptionService
    {
        string Encrypt(string plainText);
        string Decrypt(string cipherText);
    }

    public interface IUserRepository
    {
        void       Save(UserEntity user);
        UserEntity? FindByEmail(string email);
        bool       ExistsByEmail(string email);
        void       UpdateLastLogin(string email);
    }
}
