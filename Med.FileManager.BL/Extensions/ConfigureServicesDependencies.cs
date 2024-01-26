using Med.Common.Interfaces;
using Med.FileManager.BL.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Med.FileManager.BL.Extensions;

public static class ConfigureServicesDependencies
{
    public static IServiceCollection AddFileServiceDependencies(this IServiceCollection services)
    {
        services.AddScoped<IFileService, FileService>();
        return services;
    }
}
