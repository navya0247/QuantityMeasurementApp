using System.ComponentModel.DataAnnotations;

namespace AuthService.DTO
{
    public class SignUpDTO
    {
        [Required] public string FullName { get; set; } = string.Empty;
        [Required][EmailAddress] public string Email { get; set; } = string.Empty;
        [Required][MinLength(6)] public string Password { get; set; } = string.Empty;
    }

    public class SignInDTO
    {
        [Required][EmailAddress] public string Email    { get; set; } = string.Empty;
        [Required]               public string Password { get; set; } = string.Empty;
    }

    public class GoogleAuthDTO
    {
        [Required] public string IdToken { get; set; } = string.Empty;
    }

    public class UserProfileDTO
    {
        public string  Id          { get; set; } = string.Empty;
        public string  FullName    { get; set; } = string.Empty;
        public string  Email       { get; set; } = string.Empty;
        public string  Role        { get; set; } = string.Empty;
        public string  CreatedAt   { get; set; } = string.Empty;
        public string? LastLoginAt { get; set; }
    }

    public class AuthResponseDTO
    {
        public string Token            { get; set; } = string.Empty;
        public int    ExpiresInSeconds { get; set; }
        public string Email            { get; set; } = string.Empty;
        public string FullName         { get; set; } = string.Empty;
        public string Role             { get; set; } = string.Empty;
    }
}
