using Med.Backend.DAL.Data;
using Med.Common.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Med.Backend.BL.Services;

public class ConsultationService : IConsultationService
{
    private readonly ILogger<ConsultationService> _logger;
    private readonly BackendDbContext _backendDbContext;


    public ConsultationService(ILogger<ConsultationService> logger, BackendDbContext backendDbContext)
    {
        _logger = logger;
        _backendDbContext = backendDbContext;
    }
}
