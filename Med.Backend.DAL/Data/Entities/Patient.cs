using Med.Common.Enums;

namespace Med.Backend.DAL.Data.Entities;

public class Patient
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }    
    public DateTime? BirthDate { get; set; }
    public DateTime CreateDate { get; set; } = DateTime.UtcNow;
    public Gender Gender { get; set; }
    public List<Inspection>? Inspections { get; set; } = new List<Inspection>();
}
