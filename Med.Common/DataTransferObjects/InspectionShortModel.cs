namespace Med.Common.DataTransferObjects;

public class InspectionShortModel
{
    public required Guid Id { get; set; }
    public required DateTime CreateTime { get; set; }
    public required DateTime Date { get; set; }
    public required List<DiagnosisModel> DiagnosisModel { get; set; }
}
