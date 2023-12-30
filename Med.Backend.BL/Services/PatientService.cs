using Med.Backend.DAL.Data;
using Med.Common.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Med.Backend.BL.Services;

public class PatientService : IPatientService
{
    private readonly ILogger<PatientService> _logger;
    private readonly BackendDbContext _backendDbContext;


    public PatientService(ILogger<PatientService> logger, BackendDbContext backendDbContext)
    {
        _logger = logger;
        _backendDbContext = backendDbContext;
    }

}