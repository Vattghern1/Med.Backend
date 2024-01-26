using Med.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace Med.Common.DataTransferObjects;

public class InspectionPreviewModel
{
    public required Guid Id { get; set; }
    public required DateTime CreateTime { get; set; }
    public Guid? PreviousId { get; set; }
    public required Conclusion Conclusion { get; set; }
    public required Guid DoctorId { get; set; }
    [MinLength(1)]
    public required string Doctor { get; set; }
    public required Guid PatientId { get; set; }
    [MinLength(1)]
    public required string Patient { get; set; }
    public required DiagnosisModel Diagnosis { get; set; }
    public bool? HasChain { get; set; }
    public bool? HasNested { get; set; }
}
