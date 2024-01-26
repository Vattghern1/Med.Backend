using Med.Common.DataTransferObjects;
using Med.Common.Exceptions;
using Med.Common.Interfaces;
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
    private readonly IDoctorService _doctorService;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="doctorService"></param>
    public DoctorController(IDoctorService doctorService)
    {
        _doctorService = doctorService;
    }

    /// <summary>
    /// Register new user
    /// </summary>
    [HttpPost]
    [Route("register")]
    public async Task<ActionResult<TokenResponseModel>> Register([FromBody] DoctorRegisterModel doctorRegisterModel)
    {
        return Ok(await _doctorService.RegisterAsync(doctorRegisterModel, HttpContext));
    }

    /// <summary>
    /// Log in to the system
    /// </summary>
    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<TokenResponseModel>> Login([FromBody] LoginCredentialsModel accountLoginDto)
    {
        return Ok(await _doctorService.LoginAsync(accountLoginDto, HttpContext));
    }

    /// <summary>
    /// Refreshes access-token
    /// </summary>
    [HttpPost]
    [Route("refresh")]
    public async Task<ActionResult<TokenResponseModel>> Refresh([FromBody] TokenResponseModel tokenRequestDto)
    {
        return Ok(await _doctorService.RefreshTokenAsync(tokenRequestDto, HttpContext));
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
        await _doctorService.LogoutAsync(userId, HttpContext);
        return Ok();
    }

    /// <summary>
    /// Restore user password (send message with link to email)
    /// </summary>
    [HttpPost]
    [AllowAnonymous]
    [Route("generate-restore-password-link")]
    public async Task<ActionResult> RestorePassword([FromBody] EmailDto emailDto)
    {
        await _doctorService.RestorePasswordAsync(emailDto);
        return Ok();
    }

    /// <summary>
    /// Reset user's password (forgot password)
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    [Route("reset-password")]
    public async Task<ActionResult> ResetPassword(ResetPasswordDto model)
    {
        await _doctorService.ResetPasswordAsync(model);
        return Ok();
    }

    /// <summary>
    /// Get user devices
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("devices")]
    public async Task<ActionResult<List<DeviceDto>>> GetDevices()
    {
        if (User.Identity == null || Guid.TryParse(User.Identity.Name, out Guid userId) == false)
        {
            throw new UnauthorizedException("User is not authorized");
        }

        return Ok(await _doctorService.GetDevicesAsync(userId));
    }

    /// <summary>
    /// Rename device
    /// </summary>
    /// <param name="deviceId"></param>
    /// <param name="deviceRenameDto"></param>
    /// <returns></returns>
    [HttpPut]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("devices/{deviceId}")]
    public async Task<ActionResult> RenameDevice([FromRoute] Guid deviceId, [FromBody] DeviceRenameDto deviceRenameDto)
    {
        if (User.Identity == null || Guid.TryParse(User.Identity.Name, out Guid userId) == false)
        {
            throw new UnauthorizedException("User is not authorized");
        }

        await _doctorService.RenameDeviceAsync(userId, deviceId, deviceRenameDto);
        return Ok();
    }

    /// <summary>
    /// Delete device from user devices
    /// </summary>
    /// <param name="deviceId"></param>
    /// <returns></returns>
    [HttpDelete]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("devices/{deviceId}")]
    public async Task<ActionResult> DeleteDevice([FromRoute] Guid deviceId)
    {
        if (User.Identity == null || Guid.TryParse(User.Identity.Name, out Guid userId) == false)
        {
            throw new UnauthorizedException("User is not authorized");
        }

        await _doctorService.DeleteDeviceAsync(userId, deviceId);
        return Ok();
    }

    /// <summary>
    /// Get user profile
    /// </summary>
    [HttpGet]
    [Route("profile")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<ActionResult<DoctorModel>> GetUserProfile()
    {
        if (User.Identity == null || Guid.TryParse(User.Identity.Name, out Guid userId) == false)
        {
            throw new UnauthorizedException("User is not authorized");
        }

        return Ok(await _doctorService.GetProfile(userId));
    }

    /// <summary>
    /// Edit user profile
    /// </summary>
    [HttpPut]
    [Route("profile")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<ActionResult> EditUserProfile([FromBody] DoctorEditModel doctorEditModel)
    {
        if (User.Identity == null || Guid.TryParse(User.Identity.Name, out Guid userId) == false)
        {
            throw new UnauthorizedException("User is not authorized");
        }

        await _doctorService.EditProfile(doctorEditModel, userId);
        return Ok();
    }
}