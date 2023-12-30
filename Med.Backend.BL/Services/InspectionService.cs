using Med.Backend.DAL.Data;
using Med.Common.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Med.Backend.BL.Services;

public class InspectionService : IInspectionService
{
    private readonly ILogger<InspectionService> _logger;
    private readonly BackendDbContext _backendDbContext;


    public InspectionService(ILogger<InspectionService> logger, BackendDbContext backendDbContext)
    {
        _logger = logger;
        _backendDbContext = backendDbContext;
    }

}