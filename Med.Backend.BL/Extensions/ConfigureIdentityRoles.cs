using Med.Backend.DAL.Data;
using Med.Backend.DAL.Data.Entities;
using Med.Common.Enums;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Med.Backend.BL.Extensions;

public static class ConfigureIdentityRoles
{
    /// <summary>
    /// Create default roles
    /// </summary>
    /// <param name="app"></param>
    public static async Task ConfigureIdentity(this WebApplication app)
    {
        using var serviceScope = app.Services.CreateScope();

        // Migrate database
        var context = serviceScope.ServiceProvider.GetService<BackendDbContext>();

        // Get services
        var userManager = serviceScope.ServiceProvider.GetService<UserManager<User>>();
        if (userManager == null)
        {
            throw new ArgumentNullException(nameof(userManager));
        }

        var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<Role>>();
        if (roleManager == null)
        {
            throw new ArgumentNullException(nameof(roleManager));
        }

        // Try to create Roles
        foreach (var roleName in Enum.GetValues(typeof(RoleType)))
        {
            var strRoleName = roleName.ToString();
            if (strRoleName == null)
            {
                throw new ArgumentNullException(nameof(roleName), "Some role name is null");
            }

            var role = await roleManager.FindByNameAsync(strRoleName);
            if (role == null)
            {
                var roleResult =
                    await roleManager.CreateAsync(new Role
                    {
                        Name = strRoleName,
                        RoleType = (RoleType)Enum.Parse(typeof(RoleType), strRoleName),
                    });
                if (!roleResult.Succeeded)
                {
                    throw new InvalidOperationException($"Unable to create {strRoleName} role.");
                }

                role = await roleManager.FindByNameAsync(strRoleName);
            }

            if (role == null || role.Name == null)
            {
                throw new ArgumentNullException(nameof(role), "Can't find role");
            }
        }

        // Get user configuration
        var config = app.Configuration.GetSection("DefaultUsersConfig");

        if (config == null)
        {
            throw new ArgumentNullException(nameof(config), "DefaultUsersConfig is not defined");
        }
    }
}
