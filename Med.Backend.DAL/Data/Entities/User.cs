using Med.Common.Enums;
using Microsoft.AspNetCore.Identity;
using System.Security.Policy;

namespace Med.Backend.DAL.Data.Entities;

public class User : IdentityUser<Guid>
{
    public DateTime CreateTime { get; set; } = DateTime.UtcNow;
    public string FullName { get; set; }
    public DateTime? BirthDate { get; set; }
    public Gender Gender { get; set; }
    public List<Guid> UserSpecialities { get; set; } = new List<Guid>();
    public ICollection<UserRole> Roles { get; set; }
    public List<Device> Devices { get; set; } = new List<Device>();
}

