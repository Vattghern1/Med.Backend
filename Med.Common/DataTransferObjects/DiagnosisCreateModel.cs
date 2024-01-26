using Med.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Med.Common.DataTransferObjects;

public class DiagnosisCreateModel
{
    public required string IcdDiagnosisId { get; set; }
    [MaxLength(5000)]
    public string? Description { get; set; }
    public required DiagnosisType Type { get; set; }
}
