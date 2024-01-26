using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Med.Backend.DAL.Data.Entities;

public class Consultation
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreateTime { get; set; } = DateTime.UtcNow;
    public Guid SpecialityId { get; set; }
    public List<Comment> Comments { get; set; } = new List<Comment>();
    public Guid InspectionId { get; set; }
}
