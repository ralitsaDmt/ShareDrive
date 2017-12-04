using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShareDrive.Data;
using ShareDrive.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShareDrive.Infrastructure
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseDatabaseMigration(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetService<ApplicationDbContext>().Database.Migrate();

                var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<ApplicationRole>>();
                var userManager = serviceScope.ServiceProvider.GetService<UserManager<ApplicationUser>>();

                AddRoles(roleManager).Wait();
                AddAdministratorUser(userManager).Wait();
            }
            return app;
        }

        private static async Task AddAdministratorUser(UserManager<ApplicationUser> userManager)
        {
            var adminEmail = GlobalConstants.AdminMail;

            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser()
                {
                    UserName = adminEmail,
                    Email = adminEmail
                };

                await userManager.CreateAsync(adminUser, "P@ssw0rd");

                await userManager.AddToRoleAsync(adminUser, GlobalConstants.RoleAdministrator);
            }

        }

        private static async Task AddRoles(RoleManager<ApplicationRole> roleManager)
        {
            var roleNames = new List<string>() { GlobalConstants.RoleAdministrator, GlobalConstants.RoleUser };

            foreach (var roleName in roleNames)
            {
                var roleExists = await roleManager.RoleExistsAsync(roleName);

                if (!roleExists)
                {
                    await roleManager.CreateAsync(new ApplicationRole(roleName));
                }
            }
        }
    }
}
