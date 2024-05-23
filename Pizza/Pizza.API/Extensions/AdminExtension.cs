using Microsoft.AspNetCore.Identity;
using Pizza.Data;

namespace Pizza.API.Extensions
{
    public static class AdminExtension
    {
        public static void EnsureAdminExists(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            // kolla om rollen existerar
            string adminRoleName = "Admin";
            if (!roleManager.RoleExistsAsync(adminRoleName).Result)
                roleManager.CreateAsync(new IdentityRole(adminRoleName)).Wait();

            // kolla om den admin existerar
            string adminUserName = "Najah";
            var adminUser = userManager.FindByNameAsync(adminUserName).Result;
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminUserName,
                    Email = "Najahkhaderr@gmail.com",
                    PhoneNumber = "0737672656"
                };

                userManager.CreateAsync(adminUser, "Helloworld.2024").Wait();
            }

         
            if (!userManager.IsInRoleAsync(adminUser, adminRoleName).Result)
                userManager.AddToRoleAsync(adminUser, adminRoleName).Wait();
        }

    }
}