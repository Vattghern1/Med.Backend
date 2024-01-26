using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Med.Common.DataTransferObjects;

public class CommentCreateModel
{
    [MaxLength(1000)]
    [MinLength(1)]
    public required string Content { get; set; }
    public Guid? ParentId { get; set; }
}
