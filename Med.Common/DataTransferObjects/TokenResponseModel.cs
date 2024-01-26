using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Med.Common.DataTransferObjects;
public class TokenResponseModel
{
    [DisplayName("access_token")]
    public required string AccessToken { get; set; }

    [DisplayName("refresh_token")]
    public required string RefreshToken { get; set; }
}
