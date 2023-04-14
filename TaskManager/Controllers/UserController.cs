using Microsoft.AspNetCore.Mvc;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
using TaskManager.DTOs;
using TaskManager.Shared.Helpers;

namespace TaskManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;

        public UsersController(IUserService userService, IJwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Authenticate([FromBody] UserAuthenticateDto userDto)
        {
            var user = await _userService.AuthenticateAsync(userDto.Username, userDto.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            string token = await _jwtService.GenerateTokenAsync(user);
            return Ok(new { Token = token });
        }

        //[Authorize(Roles = "Admin")]

        [HttpGet("get-users")]
        public async Task<IEnumerable<User>> GetAll()
        {
            return await _userService.GetAllAsync();
        }

        [HttpPost("get-by-id")]
        public async Task<IActionResult> GetById([FromBody] GetByIdDto getByIdDto)
        {
            var user = await _userService.GetByIdAsync(getByIdDto.Id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }


        [HttpPost("create-user")]
        public async Task<IActionResult> Create([FromBody] UserCreateDto userDto)
        {
            // map dto to entity
            var user = new User
            {
                Username = userDto.Username,
                Email = userDto.Email,
                Role = userDto.Role
            };

            try
            {
                // create user
                await _userService.CreateAsync(user, userDto.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        //[Authorize]
        [HttpPut("update-user")]
        public async Task<IActionResult> Update([FromBody] UserUpdateDto userDto)
        {
            // map dto to entity and set id
            var user = new User
            {
                Id = userDto.Id,
                Username = userDto.Username,
                Email = userDto.Email,
                Role = userDto.Role
            };

            try
            {
                // update user
                await _userService.UpdateAsync(user, userDto.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        //[Authorize(Roles = "Admin")]
        [HttpDelete("delete-user")]
        public async Task<IActionResult> Delete([FromBody] DeleteDto deleteDto)
        {
            await _userService.DeleteAsync(deleteDto.Id);
            return Ok();
        }

    }
}
