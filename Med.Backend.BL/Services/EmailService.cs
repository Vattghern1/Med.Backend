using Med.Backend.DAL.Data;
using Med.Common.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Med.Backend.BL.Services;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;
    private readonly BackendDbContext _backendDbContext;


    public EmailService(ILogger<EmailService> logger, BackendDbContext backendDbContext)
    {
        _logger = logger;
        _backendDbContext = backendDbContext;
    }


}