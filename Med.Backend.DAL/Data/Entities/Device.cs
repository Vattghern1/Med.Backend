using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Med.Backend.DAL.Data.Entities;

public class Device
{
    public Guid Id { get; set; }
    public required User User { get; set; }
    public required string UserAgent { get; set; }
    public required string IpAddress { get; set; }
    public string? DeviceName { get; set; }
    public required string RefreshToken { get; set; }
    public DateTime LastActivity { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime ExpirationDate { get; set; }
}
