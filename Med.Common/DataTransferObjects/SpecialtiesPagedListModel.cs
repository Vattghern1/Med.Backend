using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Med.Common.DataTransferObjects;

public class SpecialtiesPagedListModel
{
    public List<SpecialityModel>? Specialities { get; set; }
    public PageInfoModel Pagination { get; set; }
}
