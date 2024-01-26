using Med.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace Med.Common.DataTransferObjects;

public class InspectionCreateModel
{
    public required DateTime Date { get; set; }
    [MaxLength(5000)]
    [MinLength(1)]
    public required string Anamnesis { get; set; }
    [MaxLength(5000)]
    [MinLength(1)]
    public required string Complaints { get; set; }
    [MaxLength(5000)]
    [MinLength(1)]
    public required string Treatment { get; set; }
    public required Conclusion Conclusion { get; set; }
    public DateTime? NextVisitDate { get; set; }
    public DateTime? DeathDate { get; set; }
    public Guid? PreviousInspectionId { get; set; }
    [MinLength(1)]
    public required List<DiagnosisCreateModel> Diagnoses { get; set; }
    public List<ConsultationCreateModel>? Consultations { get; set; }
}
