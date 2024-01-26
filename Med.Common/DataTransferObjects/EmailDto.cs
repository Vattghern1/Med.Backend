using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Med.Common.DataTransferObjects;
public class EmailDto
{
    /// <summary>
    /// User email
    /// </summary>
    [Required]
    [EmailAddress]
    [DisplayName("email")]
    public required string Email { get; set; }
}
