using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Med.Common.DataTransferObjects;

public class ConsultationModel
{
    public required Guid Id { get; set; }
    public required DateTime CreateTime { get; set; }
    public Guid InspectionId { get; set; }
    public SpecialityModel Speciality { get; set; }
    public List<CommentModel>? Comments { get; set; }
}
