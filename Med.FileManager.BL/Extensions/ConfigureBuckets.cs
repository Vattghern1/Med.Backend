using Med.Common.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Med.FileManager.BL.Extensions;

public static class ConfigureBuckets
{
    public static async Task CreateBuckets(this WebApplication app)
    {
        using var serviceScope = app.Services.CreateScope();
        var fileService = serviceScope.ServiceProvider.GetRequiredService<IFileService>();
        await fileService.CreateBuckets();
    }
}
