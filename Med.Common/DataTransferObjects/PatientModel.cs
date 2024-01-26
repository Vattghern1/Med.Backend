using Med.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Med.Common.DataTransferObjects;

public class PatientModel
{
    public required Guid Id { get; set; }
    public required DateTime CreateTime { get; set; }
    [MinLength(1)]
    public required string Name { get; set; }
    public DateTime? Birthday { get; set;}
    public required Gender Gender { get; set; }
}
