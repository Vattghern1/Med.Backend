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
    /// 
    /// </summary>
    [HttpGet]
    [Route("123")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<ActionResult> Get()
    {
        if (User.Identity == null || Guid.TryParse(User.Identity.Name, out Guid userId) == false)
        {
            // throw new UnauthorizedException("User is not authorized");
        }


        return Ok();
    }
}