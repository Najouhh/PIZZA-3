using Microsoft.AspNetCore.Identity;
using Pizza.Data;
using Pizza.Data.Models.DTOS.User;
using System.Threading.Tasks;

namespace Pizza.Application.Core.Interfaces
{
    public interface IUserService
    {
        Task<IdentityResult> RegisterUser(UserRegister userRegister);
        Task<List<UserGet>> GetAllUsersWithRoles();
        Task<IdentityResult> DeleteUser(string userId);
        Task<IdentityResult> UpdateUserRole(string userId, string newRole);
        Task<IdentityResult> UpdateUser(string userId, UserUpdate userUpdate);
        Task<string> Login(UserLogin userLogin);
        Task<string> GenerateTokenAsync(ApplicationUser user);
        Task<ApplicationUser> GetUserByUserName(string userName);
        Task<IdentityResult> RegisterRole(string roleName);
    }

}
