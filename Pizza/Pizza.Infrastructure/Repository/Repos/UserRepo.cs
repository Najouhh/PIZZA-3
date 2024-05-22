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
                result = await _userManager.AddToRoleAsync(user, "Regular");
            }

            return result;
        }

        public async Task<IdentityResult> RegisterRole(string roleName)
        {
            // KOLLA OM ROLLEN EXISTERAR
            if (await _roleManager.RoleExistsAsync(roleName))
                return IdentityResult.Failed(new IdentityError { Description = $"Role '{roleName}' already exists." });
            //SKAPA
            var role = new IdentityRole(roleName);
            var result = await _roleManager.CreateAsync(role);

            return result;
        }

        public async Task<List<UserGet>> GetAllUsersWithRoles()
        {
            var users = await _userManager.Users.ToListAsync(); 
            var usersWithRoles = new List<UserGet>();

            foreach (var user in users)
            {
                var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault(); 
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
            _mapper.Map(userUpdate, userToUpdate);

            return await _userManager.UpdateAsync(userToUpdate);
        }
        
        public async Task<bool> Login(UserLogin user)
        {
            // Find the user by username and check if the password is correct
            var identityUser = await _userManager.FindByNameAsync(user.UserName);
            if (identityUser == null || !await _userManager.CheckPasswordAsync(identityUser, user.Password))
            {
                // User not found or invalid password
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