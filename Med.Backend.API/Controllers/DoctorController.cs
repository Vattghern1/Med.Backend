using Med.Common.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Med.Backend.API.Controllers;

/// <summary>
/// Controller for Doctor management
/// </summary>
[ApiController]
[Route("api/doctor")]
public class DoctorController : ControllerBase
{


    private readonly ILogger<DoctorController> _logger;
    //private readonly IDoctorService _doctorService;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="logger"></param>
   // /// <param name="doctorService"></param>
    public DoctorController(ILogger<DoctorController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Register new user
    /// </summary>
    [HttpPost]
    [Route("register")]
    public async Task<ActionResult> RegisterNewUser()
    {
        return Ok();
    }

    /// <summary>
    /// Log in to the system
    /// </summary>
    [HttpPost]
    [Route("login")]
    public async Task<ActionResult> Login()
    {
        return Ok();
    }

    /// <summary>
    /// Log out system user
    /// </summary>
    [HttpPost]
    [Route("logout")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<ActionResult> Logout()
    {
        if (User.Identity == null || Guid.TryParse(User.Identity.Name, out Guid userId) == false)
        {
            throw new UnauthorizedException("User is not authorized");
        }

        return Ok();
    }

    /// <summary>
    /// Get user profile
    /// </summary>
    [HttpGet]
    [Route("profile")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<ActionResult> GetUserProfile()
    {
        if (User.Identity == null || Guid.TryParse(User.Identity.Name, out Guid userId) == false)
        {
            throw new UnauthorizedException("User is not authorized");
        }

        return Ok();
    }

    /// <summary>
    /// Edit user profile
    /// </summary>
    [HttpPut]
    [Route("profile")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<ActionResult> EditUserProfile()
    {
        if (User.Identity == null || Guid.TryParse(User.Identity.Name, out Guid userId) == false)
        {
            throw new UnauthorizedException("User is not authorized");
        }

        return Ok();
    }
}