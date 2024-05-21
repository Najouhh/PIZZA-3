using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pizza.Application.Core.Interfaces;
using Pizza.Data.Models.DTOS.User;


namespace Pizza.API.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;


        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegister userRegister)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.RegisterUser(userRegister);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok();


        }
        [HttpGet("GetAllUsersWithRoles")]
        public async Task<IActionResult> GetAllUsersWithRoles()
        {
            var usersWithRoles = await _userService.GetAllUsersWithRoles();
            return Ok(usersWithRoles);
        }
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var result = await _userService.DeleteUser(userId);
            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }
        [HttpPut("update ")]


        public async Task<IActionResult> UpdateUserRole(string userId, [FromBody] string newRole)
        {
            var result = await _userService.UpdateUserRole(userId, newRole);
            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }
        [HttpPut("updateuser")]
        public async Task<IActionResult> UpdateUser(string UserID, [FromBody] UserUpdate userUpdate)
        {
            var result = await _userService.UpdateUser(UserID, userUpdate);
            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLogin user)
        {

            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            //bool isSuccess = await _userService.Login(user);
            //if (isSuccess)

            //{
            //    _userService.GenerateToken(ApplicationUser user)
            //    return Ok("login successful");
            //}
            //else
            //{
            //    return Unauthorized("Invalid login attempt");
            //}
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var token = await _userService.Login(user);

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(new { Message = "Invalid credentials" });
            }

            return Ok(new { Token = token });
        }
    }


}
