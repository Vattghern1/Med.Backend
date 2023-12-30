using Med.Backend.DAL.Data;
using Med.Common.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Med.Backend.BL.Services;

public class DictionaryService : IDictionaryService
{
    private readonly ILogger<DictionaryService> _logger;
    private readonly BackendDbContext _backendDbContext;


    public DictionaryService(ILogger<DictionaryService> logger, BackendDbContext backendDbContext)
    {
        _logger = logger;
        _backendDbContext = backendDbContext;
    }

}