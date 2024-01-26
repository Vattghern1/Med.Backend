using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Med.Backend.DAL.Data.Entities;

public class Icd10
{
    public string Id { get; set; }
    public DateTime CreateTime { get; set; }
    public string Name { get; set; }
    public string SortField { get; set; }
    public string Code { get; set; }
    public int? ParentId { get; set; }
}
