using Med.Backend.DAL.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Med.Backend.BL.Extensions;

public static class DbMigrationExtension
{
    /// <summary>
    /// Migrate database
    /// </summary>
    /// <param name="app"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public static async Task MigrateDbAsync(this WebApplication app)
    {
        using var serviceScope = app.Services.CreateScope();

        // Migrate database
        var context = serviceScope.ServiceProvider.GetService<BackendDbContext>();
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        await context.Database.MigrateAsync();
    }
}
