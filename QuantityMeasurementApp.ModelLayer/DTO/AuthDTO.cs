using System.ComponentModel.DataAnnotations;

namespace QuantityMeasurementApp.ModelLayer.DTO
{
    /// <summary>Payload for user registration endpoint.</summary>
    public class SignUpDTO
    {
        [Required][MinLength(2)][MaxLength(100)]
        public string FullName { get; set; }

        [Required][EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$",
            ErrorMessage = "Password must have uppercase, lowercase, digit and special character.")]
        public string Password { get; set; }

        [Required][Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
    }

    /// <summary>Payload for user login endpoint.</summary>
    public class SignInDTO
    {
        [Required][EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }

    /// <summary>Returned after successful login — contains JWT token.</summary>
    public class AuthResponseDTO
    {
        public string Token            { get; set; }
        public string TokenType        { get; set; } = "Bearer";
        public int    ExpiresInSeconds { get; set; }
        public string Email            { get; set; }
        public string FullName         { get; set; }
        public string Role             { get; set; }
    }

    /// <summary>Safe user profile response — password hash never exposed.</summary>
    public class UserProfileDTO
    {
        public string Id          { get; set; }
        public string FullName    { get; set; }
        public string Email       { get; set; }
        public string Role        { get; set; }
        public string CreatedAt   { get; set; }
        public string LastLoginAt { get; set; }
    }
}
