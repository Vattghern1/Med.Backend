using System.ComponentModel.DataAnnotations;

namespace Med.Common.DataTransferObjects;

public class InspectionCommentCreateModel
{
    [MaxLength(1000)]
    [MinLength(1)]
    public required string Content { get; set; }
}
