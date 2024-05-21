

using Microsoft.AspNetCore.Identity;
using Pizza.Data;
using Pizza.Data.Models.DTOS.User;

namespace Pizza.Infrastructure.Repository.Interfaces
{
    public interface IUserRepo
    {
        Task<IdentityResult> RegisterUser(UserRegister userRegister);
        Task<List<UserGet>> GetAllUsersWithRoles();
        Task<IdentityResult> DeleteUser(string userId);
        Task<IdentityResult> UpdateUserRole(string userId, string newRole);
        Task<IdentityResult> UpdateUser(string userId, UserUpdate userUpdate);
        Task<bool> Login(UserLogin user);
        Task<ApplicationUser> GetUserByUserName(string userName);


    }
}
