using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication4.DTOs.Auth;
using WebApplication4.Models;
using WebApplication4.Services.Interfaces;

namespace WebApplication4.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _config;

        public AuthService(AppDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

        public async Task<object?> LoginAsync(LoginDto dto)
        {
            var user = await _db.Users
                .FirstOrDefaultAsync(x => x.Email == dto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
                return null;

            return new
            {
                token = GenerateJwt(user),
                IsMfaEnabled = user.IsMfaEnabled
            };
        }

        public async Task<object> ForgotPasswordAsync(ForgotPasswordDto dto)
        {
            var user = await _db.Users
                .FirstOrDefaultAsync(x => x.Email == dto.Email);

            if (user == null)
            {
                return new
                {
                    message = "If the email exists, reset link has been sent."
                };
            }

            var token = Guid.NewGuid().ToString();

            user.PasswordResetToken = token;
            user.PasswordResetTokenExpiry = DateTime.UtcNow.AddMinutes(15);

            await _db.SaveChangesAsync();

            var resetLink = $"http://localhost:4200/reset-password?token={token}";

            return new
            {
                message = "If the email exists, reset link has been sent.",
                resetLink
            };
        }

        public async Task<object> ResetPasswordAsync(ResetPasswordDto dto)
        {
            var user = await _db.Users
                .FirstOrDefaultAsync(x => x.PasswordResetToken == dto.Token);

            if (user == null || user.PasswordResetTokenExpiry <= DateTime.UtcNow)
            {
                return new
                {
                    success = false,
                    message = "Invalid or expired token"
                };
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            user.PasswordResetToken = null;
            user.PasswordResetTokenExpiry = null;

            await _db.SaveChangesAsync();

            return new
            {
                success = true,
                message = "Password reset successfully"
            };
        }

        private string GenerateJwt(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]!)
            );

            var creds = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    Convert.ToDouble(_config["Jwt:DurationInMinutes"])
                ),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}