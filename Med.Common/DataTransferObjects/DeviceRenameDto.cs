using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Med.Common.DataTransferObjects;

public class DeviceRenameDto
{
    /// <summary>
    /// Device name
    /// </summary>
    [Required]
    public required string DeviceName { get; set; }
}