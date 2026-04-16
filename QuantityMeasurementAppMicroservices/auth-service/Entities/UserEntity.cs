using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthService.Entities
{
    [Table("users")]
    public class UserEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required][Column("full_name")][MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required][Column("email")][MaxLength(200)]
        public string Email { get; set; } = string.Empty;

        [Required][Column("password_hash")]
        public string PasswordHash { get; set; } = string.Empty;

        [Column("role")][MaxLength(20)]
        public string Role { get; set; } = "User";

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [Column("auth_provider")][MaxLength(20)]
        public string AuthProvider { get; set; } = "Local";

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("last_login_at")]
        public DateTime? LastLoginAt { get; set; }
    }
}
