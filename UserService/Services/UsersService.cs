using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;
using WebApplication4.DTOs.User;
using UserService.DTOs.User;
using WebApplication4.Models;
using UserService.Services.Interfaces;

namespace UserService.Services
{
    public class UsersService : IUserService
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        private readonly IHttpClientFactory _httpClientFactory;

        public UsersService(
            AppDbContext db,
            IMapper mapper,
            IHttpClientFactory httpClientFactory)
        {
            _db = db;
            _mapper = mapper;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<UserDto> AddUserAsync(CreateUserDto dto)
        {
            var user = _mapper.Map<User>(dto);

            user.Id = Guid.NewGuid();
            user.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            user.Role = string.IsNullOrWhiteSpace(dto.Role) ? "User" : dto.Role;

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }

        //public async Task<List<UserDto>> GetUsersAsync()
        //{
        //    var users = await _db.Users.ToListAsync();
        //    return _mapper.Map<List<UserDto>>(users);
        //}
        public async Task<List<UserDto>> GetUsersAsync()
        {
            var users = await _db.Users.ToListAsync();
            var usersDto = _mapper.Map<List<UserDto>>(users);

            var client = _httpClientFactory.CreateClient("OrderClient");

            foreach (var userDto in usersDto)
            {
                try
                {
                    var response = await client.GetAsync($"api/orders/user/{userDto.Id}");

                    if (!response.IsSuccessStatusCode)
                    {
                        userDto.Orders = new List<OrderDto>();
                        continue;
                    }

                    var orders = await response.Content
                        .ReadFromJsonAsync<List<OrderDto>>();

                    userDto.Orders = orders ?? new List<OrderDto>();
                }
                catch
                {
                    userDto.Orders = new List<OrderDto>();
                }
            }

            return usersDto;
        }

        public async Task<UserDto?> GetUserAsync(Guid id)
        {
            var user = await _db.Users.FindAsync(id);

            if (user == null)
                return null;

            var userDto = _mapper.Map<UserDto>(user);

            var client = _httpClientFactory.CreateClient("OrderClient");

            try
            {
                var response = await client.GetAsync($"api/orders/user/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    userDto.Orders = new List<OrderDto>();
                    return userDto;
                }

                var orders = await response.Content
                    .ReadFromJsonAsync<List<OrderDto>>();

                userDto.Orders = orders ?? new List<OrderDto>();
            }
            catch
            {
                // If OrderService is down → still return user
                userDto.Orders = new List<OrderDto>();
            }

            return userDto;
        }

        public async Task<UserDto?> UpdateUserAsync(Guid id, UpdateUserDto dto)
        {
            var user = await _db.Users.FindAsync(id);

            if (user == null)
                return null;

            _mapper.Map(dto, user);

            await _db.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }

        // =========================
        // DELETE USER
        // =========================
        public async Task<bool> DeleteUserAsync(Guid id)
        {
            var user = await _db.Users.FindAsync(id);

            if (user == null)
                return false;

            _db.Users.Remove(user);
            await _db.SaveChangesAsync();

            return true;
        }
    }
}