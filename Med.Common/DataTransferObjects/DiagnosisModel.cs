using Med.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace Med.Common.DataTransferObjects;

public class DiagnosisModel
{
    public required Guid Id { get; set; }
    public required DateTime CreateTime { get; set; }
    [MinLength(1)]
    public required string Code { get; set; }
    [MinLength(1)]
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required DiagnosisType Type { get; set; }
}
