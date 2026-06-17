using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.DTOs.User;
using UserService.Services.Interfaces;
using WebApplication4.DTOs.User;
using WebApplication4.Models;

namespace UserService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AddUser(CreateUserDto dto)
        {
            var user = await _userService.AddUserAsync(dto);

            return CreatedAtAction(nameof(GetUser),
                new { id = user.Id },
                user);
        }

        [AllowAnonymous]
        //[Authorize(Roles = "Admin,Employee")]
        //[Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetUsersAsync();
            
            if (users == null)
                return NotFound();
            
            return Ok(users);
        }

        [AllowAnonymous]
        //[Authorize(Roles = "Admin,Employee,User")]
        [Authorize]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var user = await _userService.GetUserAsync(id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [AllowAnonymous]
        //[Authorize(Roles = "Admin")]
        //[Authorize]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateUser(Guid id, UpdateUserDto dto)
        {
            var user = await _userService.UpdateUserAsync(id, dto);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [AllowAnonymous]
        //[Authorize(Roles = "Admin")]
        //[Authorize]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var deleted = await _userService.DeleteUserAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
