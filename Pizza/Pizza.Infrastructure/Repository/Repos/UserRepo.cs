using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pizza.Data;
using Pizza.Data.Models.DTOS.User;
using Pizza.Infrastructure.Repository.Interfaces;

namespace Pizza.Infrastructure.Repository.Repos
{
    public class UserRepo : IUserRepo
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _singInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public UserRepo(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> singInManager, RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _userManager = userManager;
            _singInManager = singInManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<IdentityResult> RegisterUser(UserRegister userRegister)
        {

            var user = _mapper.Map<ApplicationUser>(userRegister);
            var result = await _userManager.CreateAsync(user, userRegister.Password);

            if (result.Succeeded)
            {
                // Ensure the "Regular" role exists
                if (!await _roleManager.RoleExistsAsync("Regular"))
                {
                    var roleResult = await _roleManager.CreateAsync(new IdentityRole("Regular"));
                    if (!roleResult.Succeeded)
                    {
                        return roleResult;
                    }
                }

                // Assign the "Regular" role to the user
                result = await _userManager.AddToRoleAsync(user, "Regular");
            }

            return result;
        }
        public async Task<List<UserGet>> GetAllUsersWithRoles()
        {
            var users = await _userManager.Users.ToListAsync(); // Assuming you're using Entity Framework Core
            var usersWithRoles = new List<UserGet>();

            foreach (var user in users)
            {
                var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault(); // Retrieve the first role (if any)
                var userGet = _mapper.Map<UserGet>(user, opts => opts.Items["Role"] = role);
                usersWithRoles.Add(userGet);
            }

            return usersWithRoles;


        }
        public async Task<IdentityResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {

                return IdentityResult.Failed(new IdentityError { Description = $"User with ID '{userId}' not found." });
            }

            var result = await _userManager.DeleteAsync(user);
            return result;
        }

        public async Task<IdentityResult> UpdateUserRole(string userId, string newRole)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = $"User with ID '{userId}' not found." });
            }

            if (!await _roleManager.RoleExistsAsync(newRole))
            {
                return IdentityResult.Failed(new IdentityError { Description = $"Role '{newRole}' not found." });
            }

            var result = await _userManager.RemoveFromRolesAsync(user, await _userManager.GetRolesAsync(user));
            if (!result.Succeeded)
            {
                return result;
            }

            return await _userManager.AddToRoleAsync(user, newRole);
        }

        public async Task<IdentityResult> UpdateUser(string userId, UserUpdate userUpdate)
        {
            var userToUpdate = await _userManager.FindByIdAsync(userId);
            if (userToUpdate == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = $"User with ID '{userId}' not found." });
            }

            // Map properties from UserUpdate to ApplicationUser
            _mapper.Map(userUpdate, userToUpdate);

            return await _userManager.UpdateAsync(userToUpdate);
        }
        public async Task<bool> Login(UserLogin user)
        {
            // Find the user by username
            var identityUser = await _userManager.FindByNameAsync(user.UserName);
            if (identityUser == null)
            {
                // User not found
                return false;
            }

            // Check if the password is correct
            var isPasswordCorrect = await _userManager.CheckPasswordAsync(identityUser, user.Password);

            if (!isPasswordCorrect)
            {
                // Invalid password
                return false;
            }

            // Sign in the user
            var result = await _singInManager.PasswordSignInAsync(user.UserName, user.Password, isPersistent: false, lockoutOnFailure: false);
            return result.Succeeded;
        }

        public async Task<ApplicationUser> GetUserByUserName(string userName)
        {
            // Correct way to find a user by username
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.UserName == userName);
            return user;
        }


    }
}