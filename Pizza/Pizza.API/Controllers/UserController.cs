using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pizza.Application.Core.Interfaces;
using Pizza.Data.Models.DTOS.User;
using System.Security.Claims;


namespace Pizza.API.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("Register a user")]
        public async Task<IActionResult> Register(UserRegister userRegister)
        {
            var result = await _userService.RegisterUser(userRegister);    
            
            if (!result.Succeeded)
            { 
                // this doesnt specify my errors

                _logger.LogWarning("User registration failed for user: {Username}. Errors: {Errors}", userRegister.Username, result.Errors);
               
                //this does
                foreach (var error in result.Errors)
                {
                    _logger.LogError("User registration error: {ErrorDescription}", error.Description);
                }

                return BadRequest(result.Errors);
            }

            _logger.LogInformation("User {Username} has been successfully registered", userRegister.Username);
            return Ok($"User {userRegister.Username} has been added");
        }


        [HttpPost("Register a role")]
        public async Task<IActionResult> RegisterRole(string roleName)
        {
            var result = await _userService.RegisterRole(roleName);

            if (result.Succeeded)
            {
                _logger.LogInformation("Role '{RoleName}' registered successfully.", roleName);
                return Ok("Role registered successfully.");
            }
            else
            {
                _logger.LogError("Error registering role '{RoleName}': {Errors}", roleName, result.Errors);
                return BadRequest(result.Errors);
            }
        }

        [HttpGet("Get All Users and their Roles")]
        public async Task<IActionResult> GetAllUsersWithRoles()
        {
            var usersWithRoles = await _userService.GetAllUsersWithRoles();
            return Ok(usersWithRoles);
        }


        [HttpPost("Delete a User")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var result = await _userService.DeleteUser(userId);

            if (result.Succeeded)
            {
                _logger.LogInformation("User with ID '{UserId}' deleted successfully.", userId);
                return Ok($"User with userid :{userId} has been deleted");
            }
            else
            {
                _logger.LogError("Error deleting user with ID '{UserId}': {Errors}", userId, result.Errors);
                return BadRequest(result.Errors);
            }
        }
        [HttpPut("update User Role ")]
        public async Task<IActionResult> UpdateUserRole(string userId, [FromBody] string newRole)
        {
            var result = await _userService.UpdateUserRole(userId, newRole);

            if (result.Succeeded)
            {       
                return Ok("Users role has been Updated");
            }
            else
            {
                _logger.LogError("Error updating user with ID '{UserId}' to role '{NewRole}': {Errors}", userId, newRole, result.Errors);
                return BadRequest(result.Errors);
            }
        }

        [Authorize]
        [HttpPost("Update User info")]
        public async Task<IActionResult> UpdateUser(string userId, [FromBody] UserUpdate userUpdate)
        {
            try
            {
                string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(currentUserId))
                {
                    _logger.LogError("Current user ID could not be found in the claims.");
                    return Unauthorized("Unable to find current user ID.");
                }

                bool isAdmin = User.IsInRole("Admin");

                if (isAdmin || currentUserId == userId)
                {
                    var result = await _userService.UpdateUser(userId, userUpdate);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User with ID '{UserId}' has been updated successfully.", userId);
                        return Ok("User has been updated.");
                    }
                    else
                    {
                        _logger.LogError("Error updating user with ID '{UserId}': {Errors}", userId, result.Errors);
                        return BadRequest(result.Errors);
                    }
                }
                else
                {
                    _logger.LogWarning("User with ID '{CurrentUserId}' attempted to update another user without sufficient permissions.", currentUserId);
                    return Forbid("You are not allowed to update other users.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while updating user with ID '{UserId}'.", userId);
                return StatusCode(500, "An unexpected error occurred while processing your request.");
            }
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLogin user)
        {

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
