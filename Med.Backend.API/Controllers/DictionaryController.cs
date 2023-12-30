using Med.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Med.Backend.API.Controllers;

/// <summary>
/// Controller for dictionary management
/// </summary>
[ApiController]
[Route("api/dictionary")]
public class DictionaryController : ControllerBase
{


    private readonly ILogger<ConsultationController> _logger;
    //private readonly IDictionaryService _dictionaryService;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="logger"></param>
   // /// <param name="dictionaryService"></param>
    public DictionaryController(ILogger<ConsultationController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Get speciality list
    /// </summary>
    [HttpGet]
    [Route("speciality")]
    public async Task<ActionResult> GetSpecialityList()
    {
        if (User.Identity == null || Guid.TryParse(User.Identity.Name, out Guid userId) == false)
        {
             throw new UnauthorizedException("User is not authorized");
        }


        return Ok();
    }

    /// <summary>
    /// Search for diagnoses in ICD-10 dictionary
    /// </summary>
    [HttpGet]
    [Route("icd10")]
    public async Task<ActionResult> SearchDiagnosesInDictionaty()
    {
        if (User.Identity == null || Guid.TryParse(User.Identity.Name, out Guid userId) == false)
        {
             throw new UnauthorizedException("User is not authorized");
        }


        return Ok();
    }

    /// <summary>
    /// Get root ICD-10 elements
    /// </summary>
    [HttpGet]
    [Route("icd/roots")]
    public async Task<ActionResult> GetRootICD10Elemements()
    {
        if (User.Identity == null || Guid.TryParse(User.Identity.Name, out Guid userId) == false)
        {
             throw new UnauthorizedException("User is not authorized");
        }

        return Ok();
    }
}