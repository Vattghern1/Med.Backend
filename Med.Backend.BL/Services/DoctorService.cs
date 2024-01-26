using Med.Backend.DAL.Data;
using Med.Backend.DAL.Data.Entities;
using Med.Common.DataTransferObjects;
using Med.Common.Enums;
using Med.Common.Exceptions;
using Med.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Web;

namespace Med.Backend.BL.Services;

public class DoctorService : IDoctorService
{
    private readonly ILogger<DoctorService> _logger;
    private readonly BackendDbContext _backendDbContext;
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;
    private readonly SignInManager<User> _signInManager;
    private readonly IEmailService _emailService;


    public DoctorService(ILogger<DoctorService> logger, BackendDbContext backendDbContext,
        UserManager<User> userManager, IConfiguration configuration, SignInManager<User> signInManager, 
        IEmailService emailService)
    {
        _logger = logger;
        _backendDbContext = backendDbContext;
        _userManager = userManager;
        _configuration = configuration;
        _signInManager = signInManager;
        _emailService = emailService;
    }

    public async Task<TokenResponseModel> RegisterAsync(DoctorRegisterModel doctorRegisterModel, HttpContext httpContext)
    {
        if (await _userManager.FindByEmailAsync(doctorRegisterModel.Email) != null)
        {
            throw new ConflictException("User with this email already exists");
        }

        if (doctorRegisterModel.Birthday >= DateTime.UtcNow)
        {
            throw new BadRequestException("Birthday more then now.");
        }

        var user = new User
        {
            UserName = doctorRegisterModel.Email,
            FullName = doctorRegisterModel.Name,
            Email = doctorRegisterModel.Email,
            Gender = doctorRegisterModel.Gender,
            PhoneNumber = doctorRegisterModel.Phone,
            BirthDate = doctorRegisterModel.Birthday
        };

        user.UserSpecialities.Add(doctorRegisterModel.Speciality);

        var result = await _userManager.CreateAsync(user, doctorRegisterModel.Password);

        if (result.Succeeded)
        {
            var newUser = await _userManager.FindByIdAsync(user.Id.ToString());

            await _userManager.AddToRoleAsync(newUser, ApplicationRoleNames.User);
            _logger.LogInformation("Successful register");
            return await LoginAsync(new LoginCredentialsModel()
            { Email = doctorRegisterModel.Email, Password = doctorRegisterModel.Password }, httpContext);
        }

        var errors = string.Join(", ", result.Errors.Select(x => x.Description));
        throw new BadRequestException(errors);
    }

    public async Task<TokenResponseModel> LoginAsync(LoginCredentialsModel accountLoginDto, HttpContext httpContext)
    {
        var identity = await GetIdentity(accountLoginDto.Email.ToLower(), accountLoginDto.Password);
        if (identity == null)
        {
            throw new BadRequestException("Incorrect username or password");
        }

        var user = _userManager.Users.Include(x => x.Devices).FirstOrDefault(x => x.Email == accountLoginDto.Email);
        if (user == null)
        {
            throw new NotFoundException("User not found");
        }

        if (await _userManager.IsLockedOutAsync(user))
        {
            throw new UnauthorizedException("User is banned");
        }

        var ipAddress = httpContext.Connection.RemoteIpAddress?.ToString() ?? "";
        var userAgent = httpContext.Request.Headers["User-Agent"].ToString();

        var device =
            user.Devices.FirstOrDefault(x => x.IpAddress == ipAddress && x.UserAgent == userAgent);

        if (device == null)
        {
            device = new Device()
            {
                User = user,
                RefreshToken = $"{Guid.NewGuid()}-{Guid.NewGuid()}",
                UserAgent = userAgent,
                IpAddress = ipAddress,
                CreatedAt = DateTime.UtcNow
            };
            await _backendDbContext.Devices.AddAsync(device);
        }

        device.LastActivity = DateTime.UtcNow;
        device.ExpirationDate = DateTime.UtcNow.AddDays(_configuration.GetSection("Jwt")
            .GetValue<int>("RefreshTokenLifetimeInDays"));

        await _backendDbContext.SaveChangesAsync();

        var jwt = new JwtSecurityToken(
            issuer: _configuration.GetSection("Jwt")["Issuer"],
            audience: _configuration.GetSection("Jwt")["Audience"],
            notBefore: DateTime.UtcNow,
            claims: identity.Claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(_configuration.GetSection("Jwt")
                .GetValue<int>("AccessTokenLifetimeInMinutes"))),
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(
                    Encoding.ASCII.GetBytes(_configuration.GetSection("Jwt")["Secret"] ?? string.Empty)),
                SecurityAlgorithms.HmacSha256));

        _logger.LogInformation("Successful login");

        return new TokenResponseModel()
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(jwt),
            RefreshToken = device.RefreshToken
        };
    }

    public async Task<TokenResponseModel> RefreshTokenAsync(TokenResponseModel tokenRequestDto, HttpContext httpContext)
    {
        tokenRequestDto.AccessToken = tokenRequestDto.AccessToken.Replace("Bearer ", "");
        var principal = GetPrincipalFromExpiredToken(tokenRequestDto.AccessToken);
        if (principal.Identity == null)
        {
            throw new BadRequestException("Invalid jwt token");
        }

        var user = _userManager.Users.Include(x => x.Devices)
            .FirstOrDefault(x => x.Id.ToString() == principal.Identity.Name);
        if (user == null)
        {
            throw new NotFoundException("User not found");
        }

        if (await _userManager.IsLockedOutAsync(user))
        {
            throw new UnauthorizedException("User is banned");
        }

        var ipAddress = httpContext.Connection.RemoteIpAddress?.ToString() ?? "";
        var userAgent = httpContext.Request.Headers["User-Agent"].ToString();

        var device =
            user.Devices.FirstOrDefault(x => x.IpAddress == ipAddress && x.UserAgent == userAgent);

        if (device == null)
        {
            throw new MethodNotAllowedException("You can't refresh token from another device. Re-login needed");
        }

        if (device.RefreshToken != tokenRequestDto.RefreshToken)
        {
            throw new BadRequestException("Refresh token is invalid");
        }

        if (device.ExpirationDate < DateTime.UtcNow)
        {
            throw new UnauthorizedException("Refresh token is expired. Re-login needed");
        }

        var jwt = new JwtSecurityToken(
            issuer: _configuration.GetSection("Jwt")["Issuer"],
            audience: null,
            notBefore: DateTime.UtcNow,
            claims: principal.Claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(_configuration.GetSection("Jwt")
                .GetValue<int>("AccessTokenLifetimeInMinutes"))),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(
                    Encoding.ASCII.GetBytes(_configuration.GetSection("Jwt")["Secret"] ?? string.Empty)),
                SecurityAlgorithms.HmacSha256));

        device.LastActivity = DateTime.UtcNow;
        device.ExpirationDate = DateTime.UtcNow.AddDays(_configuration.GetSection("Jwt")
            .GetValue<int>("RefreshTokenLifetimeInDays"));
        await _backendDbContext.SaveChangesAsync();

        return new TokenResponseModel()
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(jwt),
            RefreshToken = device.RefreshToken
        };
    }

    public async Task RestorePasswordAsync(EmailDto emailDto)
    {
        var userM = await _userManager.FindByEmailAsync(emailDto.Email);
        if (userM != null)
        {
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(userM);
            var encode = HttpUtility.UrlEncode(resetToken);
            var config = _configuration.GetSection("ResetPasswordMVCUrl");
            try
            {
                await _emailService.SendEmailAsync(emailDto.Email, "Восстановление пароля",
                    $"Ваш запрос на сброс пароля получен.<br />Пожалуйста, посетите следующий URL-адрес, чтобы сбросить пароль: <a href='{config.GetValue<string>("Url")}?userId={userM.Id}&code={encode}'>link</a>");
            }
            catch (Exception e)
            {
                throw new BadRequestException(e.Message);
            }
        }
        else
        {
            throw new BadRequestException("User not found or email is not confirmed.");
        }
    }

    /// <summary>
    /// Reset password
    /// </summary>
    /// <param name="model"></param>
    public async Task ResetPasswordAsync(ResetPasswordDto model)
    {
        var userM = await _userManager.FindByEmailAsync(model.Email);
        if (userM == null)
        {
            throw new NotFoundException("User was not found.");
        }

        var result =
            await _userManager.ResetPasswordAsync(userM, HttpUtility.UrlDecode(model.Token), model.NewPassword);
        if (!result.Succeeded)
        {
            throw new ConflictException(string.Join(", ", result.Errors.Select(x => x.Description)));
        };
    }

    private ClaimsPrincipal GetPrincipalFromExpiredToken(string jwtToken)
    {
        var key = new SymmetricSecurityKey(
            Encoding.ASCII.GetBytes(_configuration.GetSection("Jwt")["Secret"] ?? string.Empty));

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,
            ValidateIssuer = true,
            ValidIssuer = _configuration.GetSection("Jwt")["Issuer"],
            ValidateAudience = true,
            ValidAudience = _configuration.GetSection("Jwt")["Audience"],
            ValidateLifetime = false
        };

        ClaimsPrincipal principal;
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            principal = tokenHandler.ValidateToken(jwtToken, validationParameters, out var securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
        }
        catch (ArgumentException ex)
        {
            throw new BadRequestException("Invalid jwt token", ex);
        }

        return principal;
    }

    public async Task LogoutAsync(Guid userId, HttpContext httpContext)
    {
        var user = _userManager.Users
            .Include(x => x.Devices)
            .FirstOrDefault(x => x.Id == userId);
        if (user == null)
        {
            throw new NotFoundException("User not found");
        }

        var ipAddress = httpContext.Connection.RemoteIpAddress?.ToString() ?? "";
        var userAgent = httpContext.Request.Headers["User-Agent"].ToString();

        var device =
            user.Devices.FirstOrDefault(x => x.IpAddress == ipAddress && x.UserAgent == userAgent);

        if (device == null)
        {
            throw new MethodNotAllowedException("You can`t logout from this device");
        }

        _backendDbContext.Devices.Remove(device);
        await _backendDbContext.SaveChangesAsync();
    }

    public Task<List<DeviceDto>> GetDevicesAsync(Guid userId)
    {
        var user = _userManager.Users.Include(x => x.Devices).FirstOrDefault(u => u.Id == userId);
        if (user == null)
        {
            throw new NotFoundException("User not found");
        }

        return Task.FromResult(user.Devices.Select(d => new DeviceDto
        {
            DeviceName = d.DeviceName,
            IpAddress = d.IpAddress,
            UserAgent = d.UserAgent,
            LastActivity = d.LastActivity,
            Id = d.Id,
        }).ToList());
    }

    public async Task RenameDeviceAsync(Guid userId, Guid deviceId, DeviceRenameDto deviceRenameDto)
    {
        var user = _userManager.Users
            .Include(x => x.Devices)
            .FirstOrDefault(u => u.Id == userId);
        if (user == null)
        {
            throw new NotFoundException("User not found");
        }

        var device = user.Devices.FirstOrDefault(d => d.Id == deviceId);
        if (device == null)
        {
            throw new NotFoundException("Device not found");
        }

        device.DeviceName = deviceRenameDto.DeviceName;
        await _backendDbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Delete device
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="deviceId"></param>
    public async Task DeleteDeviceAsync(Guid userId, Guid deviceId)
    {
        var user = _userManager.Users.FirstOrDefault(u => u.Id == userId);
        if (user == null)
        {
            throw new NotFoundException("User not found");
        }

        var device = _backendDbContext.Devices.FirstOrDefault(d => d.User == user);
        if (device == null)
        {
            throw new NotFoundException("Device not found");
        }

        _backendDbContext.Devices.Remove(device);
        await _backendDbContext.SaveChangesAsync();
    }

    private async Task<ClaimsIdentity?> GetIdentity(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return null;
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
        if (!result.Succeeded) return null;

        var claims = new List<Claim> {
            new Claim(ClaimTypes.Name, user.Id.ToString())
        };

        foreach (var role in await _userManager.GetRolesAsync(user))
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        return new ClaimsIdentity(claims, "Token", ClaimTypes.Name, ClaimTypes.Role);
    }

    public async Task<DoctorModel> GetProfile(Guid userId)
    {
        var userM = await _userManager.FindByIdAsync(userId.ToString());
        var user = await _backendDbContext.Users
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            throw new NotFoundException("User not found");
        }

        var profile = new DoctorModel
        {
            Id = userId,
            CreateTime = user.CreateTime,
            Name = user.FullName,
            Birthday = user.BirthDate,
            Gender = user.Gender,
            Email = user.Email,
            Phone = user.PhoneNumber
        };
        return profile;
    }

    public async Task EditProfile(DoctorEditModel doctorEditModel, Guid userId)
    {
        var user = await _backendDbContext.Users
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            throw new NotFoundException("User not found");
        }

        user.Email = doctorEditModel.Email;
        user.UserName = doctorEditModel.Email;
        user.FullName = doctorEditModel.Name;
        user.BirthDate = doctorEditModel.Birthday;
        user.Gender = doctorEditModel.Gender;
        user.PhoneNumber = doctorEditModel.Phone;

        _backendDbContext.UpdateRange(user);
        await _backendDbContext.SaveChangesAsync();
    }
}