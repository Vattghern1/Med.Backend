using Med.Common.Exceptions;
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


    private readonly ILogger<InspectionController> _logger;
    //private readonly IInspectionService _inspectionService;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="logger"></param>
   // /// <param name="inspectionService"></param>
    public InspectionController(ILogger<InspectionController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Get full information about specified inspection
    /// </summary>
    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult> GetInfoAboutInspection()
    {
        if (User.Identity == null || Guid.TryParse(User.Identity.Name, out Guid userId) == false)
        {
            throw new UnauthorizedException("User is not authorized");
        }

        return Ok();
    }

    /// <summary>
    /// Edit concrete inspection
    /// </summary>
    [HttpPut]
    [Route("{id}")]
    public async Task<ActionResult> EditConcreteInspection()
    {
        if (User.Identity == null || Guid.TryParse(User.Identity.Name, out Guid userId) == false)
        {
            throw new UnauthorizedException("User is not authorized");
        }

        return Ok();
    }

    /// <summary>
    /// Get medical inspection chain for root inspection
    /// </summary>
    [HttpGet]
    [Route("{id}/chain")]
    public async Task<ActionResult> GetInspectionChain()
    {
        if (User.Identity == null || Guid.TryParse(User.Identity.Name, out Guid userId) == false)
        {
            throw new UnauthorizedException("User is not authorized");
        }

        return Ok();
    }
}