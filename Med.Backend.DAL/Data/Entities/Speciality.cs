using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Med.Backend.DAL.Data.Entities;

public class Speciality
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreateTime { get; set; } = DateTime.UtcNow;    
    public string Name { get; set; }
}
