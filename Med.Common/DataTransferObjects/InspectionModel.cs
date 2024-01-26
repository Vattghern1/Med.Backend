using Med.Common.Enums;

namespace Med.Common.DataTransferObjects;

public class InspectionModel
{
    public required Guid InspectionId { get; set; }
    public required DateTime CreateTime { get; set; }
    public required DateTime Date { get; set; }
    public string? Anamnesis { get; set; }
    public string? Complaints { get; set; }
    public string? Treatment { get; set; }
    public required Conclusion Conclusion { get; set; }
    public DateTime? NextVisitDate { get; set; }
    public DateTime? DeathDate { get; set; }
    public Guid? BaseInspectionId { get; set; }
    public Guid? PreviousInspectionId { get; set; }
    public required PatientModel Patient { get; set; }
    public required DoctorModel Doctor { get; set; }
    public List<DiagnosisModel>? Diagnoses { get; set; }
    public List<InspectionConsultationModel>? Consultations { get; set; }

}
