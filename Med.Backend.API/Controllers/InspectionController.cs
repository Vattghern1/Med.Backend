using Med.Common.DataTransferObjects;
using Med.Common.Exceptions;
using Med.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Med.Backend.API.Controllers;

/// <summary>
/// Controller for Inspection management
/// </summary>
[ApiController]
[Route("api/inspection")]
[Authorize(AuthenticationSchemes = "Bearer")]
public class InspectionController : ControllerBase
{
    private readonly IInspectionService _inspectionService;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="inspectionService"></param>
    public InspectionController(IInspectionService inspectionService)
    {
        _inspectionService = inspectionService;
    }

    /// <summary>
    /// Get full information about specified inspection
    /// </summary>
    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<InspectionModel>> GetInfoAboutInspection(Guid id)
    {
        if (User.Identity == null || Guid.TryParse(User.Identity.Name, out Guid userId) == false)
        {
            throw new UnauthorizedException("User is not authorized");
        }

        return Ok(await _inspectionService.GetInfoAboutInspection(id));
    }

    /// <summary>
    /// Edit concrete inspection
    /// </summary>
    [HttpPut]
    [Route("{id}")]
    public async Task<ActionResult> EditConcreteInspection([FromBody] InspectionEditModel inspectionEditModel, Guid id)
    {
        if (User.Identity == null || Guid.TryParse(User.Identity.Name, out Guid userId) == false)
        {
            throw new UnauthorizedException("User is not authorized");
        }

        await _inspectionService.EditConcreteInspection(inspectionEditModel, id, userId);
        return Ok();
    }

    /// <summary>
    /// Get medical inspection chain for root inspection
    /// </summary>
    [HttpGet]
    [Route("{id}/chain")]
    public async Task<ActionResult<List<InspectionPreviewModel>>> GetInspectionChain(Guid id)
    {
        if (User.Identity == null || Guid.TryParse(User.Identity.Name, out Guid userId) == false)
        {
            throw new UnauthorizedException("User is not authorized");
        }

        return Ok(await _inspectionService.GetInspectionChain(id));
    }
}