using AutoMapper;
using UserService.DTOs.User;
using WebApplication4.DTOs.User;

namespace UserService.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> AddUserAsync(CreateUserDto dto);
        Task<List<UserDto>> GetUsersAsync();
        Task<UserDto?> GetUserAsync(Guid id);
        Task<UserDto?> UpdateUserAsync(Guid id, UpdateUserDto dto);
        Task<bool> DeleteUserAsync(Guid id);
    }
}
