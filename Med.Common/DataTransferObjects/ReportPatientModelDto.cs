using Med.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Med.Common.DataTransferObjects;

public class ReportPatientModelDto
{
    public string Name { get; set; }
    public Guid Id { get; set; }
    public DateTime? BirthDate { get; set; }
    public Gender Gender { get; set; }
}
