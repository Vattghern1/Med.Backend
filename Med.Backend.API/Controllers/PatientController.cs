using Med.Common.DataTransferObjects;
using Med.Common.Enums;
using Med.Common.Exceptions;
using Med.Common.Interfaces;
using Med.Common.Other;
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
    private readonly IPatientService _patientService;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="patientService"></param>
    public PatientController(IPatientService patientService)
    {
        _patientService = patientService;
    }

    /// <summary>
    /// Create new patient
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateNewPatient([FromBody] PatientCreateModel patientCreateModel)
    {
        if (User.Identity == null || Guid.TryParse(User.Identity.Name, out Guid userId) == false)
        {
            throw new UnauthorizedException("User is not authorized");
        }

        return Ok(await _patientService.CreateNewPatient(patientCreateModel));
    }

    /// <summary>
    /// Get patients list
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<PagedList<PatientModel>>> GetPatientList([FromQuery] string? name,
        [FromQuery] List<Conclusion>? conclusions,
        [FromQuery] Sorting? sorting,
        [FromQuery] bool? scheduledVisits,
        [FromQuery] bool? onlyMine,
        [FromQuery] int page,
        [FromQuery] int size)
    {
        if (User.Identity == null || Guid.TryParse(User.Identity.Name, out Guid userId) == false)
        {
            throw new UnauthorizedException("User is not authorized");
        }

        return Ok(await _patientService.GetPatientList(name, conclusions, sorting, scheduledVisits, onlyMine, page, size, userId));
    }

    /// <summary>
    /// Create inspection for specified patient
    /// </summary>
    [HttpPost]
    [Route("{id}/inspections")]
    public async Task<ActionResult<Guid>> CreateInspectionForPatient([FromBody] InspectionCreateModel inspectionCreateModel, Guid id)
    {
        if (User.Identity == null || Guid.TryParse(User.Identity.Name, out Guid userId) == false)
        {
            throw new UnauthorizedException("User is not authorized");
        }

        return Ok(await _patientService.CreateInspectionForPatient(inspectionCreateModel, id, userId));
    }

    /// <summary>
    /// Get a list of patient medical inspections
    /// </summary>
    [HttpGet]
    [Route("{id}/inspections")]
    public async Task<ActionResult<PagedList<InspectionPreviewModel>>> GetListOfPatientInspections(Guid id,
        [FromQuery] bool grouped,
        [FromQuery] List<string>? icdRoots,
        [FromQuery] int page,
        [FromQuery] int size)
    {
        if (User.Identity == null || Guid.TryParse(User.Identity.Name, out Guid userId) == false)
        {
            throw new UnauthorizedException("User is not authorized");
        }

        return Ok(await _patientService.GetListOfPatientInspections(id, grouped, icdRoots, page, size));
    }

    /// <summary>
    /// Get patient card
    /// </summary>
    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<PatientModel>> GetPatientCard(Guid id)
    {
        if (User.Identity == null || Guid.TryParse(User.Identity.Name, out Guid userId) == false)
        {
            throw new UnauthorizedException("User is not authorized");
        }

        return Ok(await _patientService.GetPatientCard(id));
    }

    /// <summary>
    /// Search for patient medical inspections without child inspections
    /// </summary>
    [HttpGet]
    [Route("{id}/inspections/search")]
    public async Task<ActionResult<List<InspectionShortModel>>> SearchForPatientInspections([FromQuery] string? request, Guid id)
    {
        if (User.Identity == null || Guid.TryParse(User.Identity.Name, out Guid userId) == false)
        {
            throw new UnauthorizedException("User is not authorized");
        }

        return Ok(await _patientService.SearchForPatientInspections(request, id));
    }
}