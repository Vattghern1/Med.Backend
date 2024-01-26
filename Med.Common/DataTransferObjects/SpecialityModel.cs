using System.ComponentModel.DataAnnotations;

namespace Med.Common.DataTransferObjects;

public class SpecialityModel
{
    public required Guid Id { get; set; }
    public required DateTime CreateTime { get; set; }
    [MinLength(1)]
    public required string Name { get; set; }
}
