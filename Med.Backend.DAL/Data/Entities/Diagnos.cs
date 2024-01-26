using Med.Common.Enums;

namespace Med.Backend.DAL.Data.Entities;

public class Diagnos
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreateTime { get; set; } = DateTime.UtcNow;
    public string IcdDiagnosisId { get; set; }
    public string DiagnosName { get; set; }
    public string Description { get; set; }
    public DiagnosisType DiagnosisType { get; set; }
    public required Inspection Inspection { get; set; }
}
