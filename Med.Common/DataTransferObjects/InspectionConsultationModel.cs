using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Med.Common.DataTransferObjects;

public class InspectionConsultationModel
{
    public required Guid ConsultationId { get; set; }
    public required Guid InspectionId { get; set; }
    public required DateTime CreateTime { get; set; }
    public required SpecialityModel Speciality { get; set; }
    public InspectionCommentModel? RootComment { get; set; }
    public int CommentsNumber { get; set; }
}
