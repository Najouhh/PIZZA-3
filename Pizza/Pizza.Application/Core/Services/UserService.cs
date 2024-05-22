

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Pizza.Application.Core.Interfaces;
using Pizza.Data;
using Pizza.Data.Models.DTOS.User;
using Pizza.Infrastructure.Repository.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Pizza.Application.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserService(IUserRepo userRepo, IConfiguration configuration, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userRepo = userRepo;
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> RegisterUser(UserRegister userRegister)
        {
            return await _userRepo.RegisterUser(userRegister);
        }
        public async Task<IdentityResult> RegisterRole(string roleName)
        {
            return await _userRepo.RegisterRole(roleName);
        }
        public async Task<ApplicationUser> GetUserByUserName(string userName)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.UserName == userName);
            return user;
        }
       
        public async Task<List<UserGet>> GetAllUsersWithRoles()
        {
            return await _userRepo.GetAllUsersWithRoles();
        }

        public async Task<IdentityResult> DeleteUser(string userId)
        {
            return await _userRepo.DeleteUser(userId);
        }

        public async Task<IdentityResult> UpdateUserRole(string userId, string newRole)
        {
            return await _userRepo.UpdateUserRole(userId, newRole);
        }

        public async Task<IdentityResult> UpdateUser(string userId, UserUpdate userUpdate)
        {
            return await _userRepo.UpdateUser(userId, userUpdate);
        }

        public async Task<string> Login(UserLogin userLogin)
        {
            var user = await _userManager.FindByNameAsync(userLogin.UserName);
            if (user == null)
            {
                return null; // User not found
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, userLogin.Password, false);
            if (!result.Succeeded)
            {
                return null; // Invalid credentials
            }

            return await GenerateTokenAsync(user);
        }

        public async Task<string> GenerateTokenAsync(ApplicationUser user)
        {
            // Retrieve user roles from UserManager
            var roles = await _userManager.GetRolesAsync(user);

            // Define the claims to be included in the JWT token
            var authClaims = new List<Claim>
              {
                 new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                 new Claim(ClaimTypes.Name, user.UserName)
              };

            // Add role claims
            authClaims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            // Define the signing key using the secret key from configuration
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            // Create the JWT token
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            // Return the serialized token
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
