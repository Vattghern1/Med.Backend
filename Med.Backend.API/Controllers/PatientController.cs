using Med.Common.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Med.Backend.API.Controllers;

/// <summary>
/// Controller for Patient management
/// </summary>
[ApiController]
[Route("api/patient")]
[Authorize(AuthenticationSchemes = "Bearer")]
public class PatientController : ControllerBase
{


    private readonly ILogger<PatientController> _logger;
    //private readonly IPatientService _patientService;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="logger"></param>
   // /// <param name="patientService"></param>
    public PatientController(ILogger<PatientController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Create new patient
    /// </summary>
    [HttpPost]
    public async Task<ActionResult> CreateNewPatient()
    {
        if (User.Identity == null || Guid.TryParse(User.Identity.Name, out Guid userId) == false)
        {
            throw new UnauthorizedException("User is not authorized");
        }

        return Ok();
    }

    /// <summary>
    /// Get patients list
    /// </summary>
    [HttpGet]
    public async Task<ActionResult> GetPatientList()
    {
        if (User.Identity == null || Guid.TryParse(User.Identity.Name, out Guid userId) == false)
        {
            throw new UnauthorizedException("User is not authorized");
        }

        return Ok();
    }

    /// <summary>
    /// Create inspection for specified patient
    /// </summary>
    [HttpPost]
    [Route("{id}/inspections")]
    public async Task<ActionResult> CreateInspectionForPatient()
    {
        if (User.Identity == null || Guid.TryParse(User.Identity.Name, out Guid userId) == false)
        {
            throw new UnauthorizedException("User is not authorized");
        }

        return Ok();
    }

    /// <summary>
    /// Get a list of patient medical inspections
    /// </summary>
    [HttpGet]
    [Route("{id}/inspections")]
    public async Task<ActionResult> GetListOfPatientInspections()
    {
        if (User.Identity == null || Guid.TryParse(User.Identity.Name, out Guid userId) == false)
        {
            throw new UnauthorizedException("User is not authorized");
        }

        return Ok();
    }

    /// <summary>
    /// Get patient card
    /// </summary>
    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult> GetPatientCard()
    {
        if (User.Identity == null || Guid.TryParse(User.Identity.Name, out Guid userId) == false)
        {
            throw new UnauthorizedException("User is not authorized");
        }

        return Ok();
    }

    /// <summary>
    /// Search for patient medical inspections without child inspections
    /// </summary>
    [HttpGet]
    [Route("{id}/inspections/search")]
    public async Task<ActionResult> SearchForPatientInspections()
    {
        if (User.Identity == null || Guid.TryParse(User.Identity.Name, out Guid userId) == false)
        {
            throw new UnauthorizedException("User is not authorized");
        }

        return Ok();
    }
}