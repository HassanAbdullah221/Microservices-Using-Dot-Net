using System.ComponentModel.DataAnnotations;

namespace WebApplication4.Models
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required, MaxLength(150)]
        public string FullName { get; set; } 

        [Required, EmailAddress, MaxLength(200)]
        public string Email { get; set; }

        [Required, MaxLength(100)]
        public string Department { get; set; } 

        [Required, Phone, MaxLength(30)]
        public string PhoneNumber { get; set; }

        [Required]
        [MaxLength(100)]
        public string Password { get; set; }

        [Required]
        [MaxLength(20)]
        public string Role { get; set; } = "User";

        public string? PasswordResetToken { get; set; }
        public DateTime? PasswordResetTokenExpiry { get; set; }

        public string? MfaSecret { get; set; }
        public bool IsMfaEnabled { get; set; } = false;


    }
}
