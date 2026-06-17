using WebApplication4.DTOs.Auth;

namespace WebApplication4.Services.Interfaces
{
    public interface IAuthService
    {
        Task<object?> LoginAsync(LoginDto dto);
        Task<object> ForgotPasswordAsync(ForgotPasswordDto dto);
        Task<object> ResetPasswordAsync(ResetPasswordDto dto);
    }
}