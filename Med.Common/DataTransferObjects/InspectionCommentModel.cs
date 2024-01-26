namespace Med.Common.DataTransferObjects;

public class InspectionCommentModel
{
    public required Guid Id { get; set; }
    public required DateTime CreateTime { get; set; }
    public Guid? ParentId { get; set; }
    public string? Content { get; set; }
    public required DoctorModel Author { get; set; }
    public DateTime? ModifyTime { get; set; }
}
