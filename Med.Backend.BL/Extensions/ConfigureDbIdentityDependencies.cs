using Med.Backend.DAL.Data;
using Med.Backend.DAL.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Med.Backend.BL.Extensions;

public static class ConfigureDbIdentityDependencies
{
    public static IServiceCollection AddIdentityManagers(this IServiceCollection services)
    {
        services.AddIdentity<User, Role>()
            .AddEntityFrameworkStores<BackendDbContext>()
            .AddDefaultTokenProviders()
            .AddSignInManager<SignInManager<User>>()
            .AddUserManager<UserManager<User>>()
            .AddRoleManager<RoleManager<Role>>();
        return services;
    }
}