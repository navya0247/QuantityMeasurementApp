using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuantityMeasurementApp.ModelLayer.Entities
{
    /// <summary>
    /// EF Core entity mapped to the users table.
    ///
    /// Columns:
    ///   password_hash — BCrypt hash of (password + salt)
    ///   password_salt — the random salt generated at registration
    ///                   Both are needed together to verify a login attempt.
    /// </summary>
    [Table("users")]
    public class UserEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required][Column("full_name")][MaxLength(100)]
        public string FullName { get; set; }

        [Required][Column("email")][MaxLength(200)]
        public string Email { get; set; }

        /// <summary>
        /// BCrypt hash of (plainPassword + PasswordSalt).
        /// Never store or log the plain password.
        /// </summary>
        [Required][Column("password_hash")]
        public string PasswordHash { get; set; }


        [Column("role")][MaxLength(20)]
        public string Role { get; set; } = "User";

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("last_login_at")]
        public DateTime? LastLoginAt { get; set; }
    }
}