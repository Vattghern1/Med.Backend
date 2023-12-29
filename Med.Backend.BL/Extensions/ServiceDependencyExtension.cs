using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Med.Backend.BL.Extensions;

public static class ServiceDependencyExtension
{
    public static IServiceCollection AddBackendServices(this IServiceCollection services, IConfiguration configuration)
    {
        // services.AddDbContext<BackendDbContext>(options =>
        //     options.UseNpgsql(configuration.GetConnectionString("BackendDatabase")));
        // services.AddScoped<IConsultationService, ConsultationService>();

        return services;
    }
}
