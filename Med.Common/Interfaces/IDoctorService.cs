using Med.Common.DataTransferObjects;
using Microsoft.AspNetCore.Http;

namespace Med.Common.Interfaces;

public interface IDoctorService
{
    Task<TokenResponseModel> RegisterAsync(DoctorRegisterModel doctorRegisterModel, HttpContext httpContext );
    Task<TokenResponseModel> LoginAsync(LoginCredentialsModel accountLoginDto, HttpContext httpContext);
    Task LogoutAsync(Guid userId, HttpContext httpContext);
    Task<DoctorModel> GetProfile(Guid userId);
    Task EditProfile(DoctorEditModel doctorEditModel, Guid userId);
    Task<TokenResponseModel> RefreshTokenAsync(TokenResponseModel tokenRequestDto, HttpContext httpContext);
    Task<List<DeviceDto>> GetDevicesAsync(Guid userId);
    Task RenameDeviceAsync(Guid userId, Guid deviceId, DeviceRenameDto deviceRenameDto);
    Task DeleteDeviceAsync(Guid userId, Guid deviceId);
    Task RestorePasswordAsync(EmailDto emailDto);
    Task ResetPasswordAsync(ResetPasswordDto model);
}