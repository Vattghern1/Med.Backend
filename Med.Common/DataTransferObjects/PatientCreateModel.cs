using Med.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Med.Common.DataTransferObjects;

public class PatientCreateModel
{
    [MaxLength(1000)]
    [MinLength(1)]
    public required string Name { get; set; }
    public DateTime? Birthday { get; set; }
    public required Gender Gender { get; set; }
}
