using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Med.Backend.DAL.Data.Entities;

/// <summary>
/// Entity for Consultation
/// </summary>
public class Consultation
{
    /// <summary>
    /// Consultation id
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();
    /// <summary>
    /// Date and time the Consultation was created
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
