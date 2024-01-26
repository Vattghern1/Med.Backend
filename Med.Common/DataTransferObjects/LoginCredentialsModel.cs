using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Med.Common.DataTransferObjects;

public class LoginCredentialsModel
{
    [EmailAddress]
    [MinLength(1)]
    public required string Email { get; set; }
    [MinLength(1)] 
    public required string Password { get; set; }
}
