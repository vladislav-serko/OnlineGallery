using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnlineGallery.DAL.Models;

namespace OnlineGallery.BLL.Helpers.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static async Task SeedUsersAndRoles(this IApplicationBuilder builder, IServiceProvider provider)
        {
            var userManager = provider.GetService<UserManager<User>>();
            var roleManager = provider.GetService<RoleManager<IdentityRole>>();
            var configuration = provider.GetService<IConfiguration>();

            await AddRoles(roleManager);
            await AddAdmin(userManager, configuration);
        }

        private static async Task AddRoles(RoleManager<IdentityRole> roleManager)
        {
            var roleNames = new[] {"Admin", "Moderator", "User"};
            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                    await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        private static async Task AddAdmin(UserManager<User> userManager, IConfiguration configuration)
        {
            var admin = new User
            {
                UserName = configuration["AdminSettings:Username"]
            };

            var user = await userManager.FindByNameAsync(admin.UserName);
            if (user == null)
            {
                var result = await userManager.CreateAsync(admin, configuration["AdminSettings:Password"]);
                if (result.Succeeded)
                    await userManager.AddToRolesAsync(admin, new[]
                    {
                        "Admin", "User"
                    });
            }
        }
    }
}