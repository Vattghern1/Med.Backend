using Med.Common.Exceptions;
using Med.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace Med.Backend.API.Controllers;

/// <summary>
/// Controller for consultation management
/// </summary>
[ApiController]
[Route("api/consultation")]
[Authorize(AuthenticationSchemes = "Bearer")]
public class ConsultationController : ControllerBase
{


    private readonly ILogger<ConsultationController> _logger;
    private readonly IConsultationService _consultationService;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="consultationService"></param>
    public ConsultationController(ILogger<ConsultationController> logger, IConsultationService consultationService)
    {
        _logger = logger;
        _consultationService = consultationService;
    }

    /// <summary>
    /// Get a list of medical inspections for consultation
    /// </summary>
    [HttpGet]
    public async Task<ActionResult> GetConsultationList()
    {
        if (User.Identity == null || Guid.TryParse(User.Identity.Name, out Guid userId) == false)
        {
             throw new UnauthorizedException("User is not authorized");
        }
        return Ok();
    }

    /// <summary>
    /// Get concrete consultation
    /// </summary>
    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult> GetConsultation(Guid id)
    {
        if (User.Identity == null || Guid.TryParse(User.Identity.Name, out Guid userId) == false)
        {
            throw new UnauthorizedException("User is not authorized");
        }
        return Ok();
    }

    /// <summary>
    /// Add comment to concrete consultation
    /// </summary>
    [HttpPost]
    [Route("{id}/comment")]
    public async Task<ActionResult> AddCommentToConsultation(Guid id)
    {
        if (User.Identity == null || Guid.TryParse(User.Identity.Name, out Guid userId) == false)
        {
            throw new UnauthorizedException("User is not authorized");
        }
        return Ok();
    }

    /// <summary>
    /// Edit comment
    /// </summary>
    [HttpPut]
    [Route("comment/{id}")]
    public async Task<ActionResult> EditConsultationComment(Guid id)
    {
        if (User.Identity == null || Guid.TryParse(User.Identity.Name, out Guid userId) == false)
        {
            throw new UnauthorizedException("User is not authorized");
        }
        return Ok();
    }
}