using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Med.Common.DataTransferObjects;

public class PaginationParamsDto
{
    public string? Name { get; set; }
    [DefaultValue(1)]
    public int Page { get; set; }
    [DefaultValue(5)]
    public int Size { get; set; }
}
