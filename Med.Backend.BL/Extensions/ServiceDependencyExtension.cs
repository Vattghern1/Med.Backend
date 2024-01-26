using Med.Backend.BL.Services;
using Med.Backend.DAL.Data;
using Med.Common.Interfaces;
using Med.FileManager.BL.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Med.Backend.BL.Extensions;

public static class ServiceDependencyExtension
{
    public static IServiceCollection AddBackendServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<BackendDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("BackendDatabase")));

        services.AddScoped<IDoctorService, DoctorService>();
        services.AddScoped<IPatientService, PatientService>();
        services.AddScoped<IConsultationService, ConsultationService>();
        services.AddScoped<IDictionaryService, DictionaryService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IInspectionService, InspectionService>();
        services.AddScoped<IReportService, ReportService>();
        services.AddScoped<IFileService, FileService>();

        return services;
    }
}
