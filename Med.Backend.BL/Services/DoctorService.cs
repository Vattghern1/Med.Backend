using Med.Backend.DAL.Data;
using Med.Common.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Med.Backend.BL.Services;

public class DoctorService : IDoctorService
{
    private readonly ILogger<DoctorService> _logger;
    private readonly BackendDbContext _backendDbContext;


    public DoctorService(ILogger<DoctorService> logger, BackendDbContext backendDbContext)
    {
        _logger = logger;
        _backendDbContext = backendDbContext;
    }

}