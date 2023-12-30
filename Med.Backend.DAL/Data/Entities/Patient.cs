using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Med.Backend.DAL.Data.Entities;

/// <summary>
/// Entity for Patient
/// </summary>
public class Patient
{
    /// <summary>
    /// Patient id
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();
    /// <summary>
    /// Patient`s birth date
    /// </summary>
    public DateTime BirthDate { get; set; }
}
