using Med.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace Med.Common.DataTransferObjects;
public class DoctorRegisterModel
{
    [MinLength(1)]
    [MaxLength(1000)]
    public required string Name { get; set; }
    [MinLength(6)]
    public required string Password { get; set; }
    [EmailAddress]
    public required string Email { get; set; }
    public DateTime? Birthday { get; set; }
    public required Gender Gender { get; set; }
    [Phone]
    public string? Phone { get; set; }
    public required Guid Speciality { get; set; }
}
