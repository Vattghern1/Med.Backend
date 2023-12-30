using System.ComponentModel.DataAnnotations;

namespace Med.Common.DataTransferObjects;
public class TokenResponseModel
{
    [MinLength(1)]
    public required string Token { get; set; }
}
