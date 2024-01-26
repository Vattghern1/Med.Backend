using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Med.Common.DataTransferObjects;

public class ConsultationCreateModel
{
    public required Guid SpecialityId { get; set; } 
    public required InspectionCommentCreateModel Comment { get; set; }
}
