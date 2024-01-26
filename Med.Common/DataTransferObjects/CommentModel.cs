using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Med.Common.DataTransferObjects;

public class CommentModel
{
    public required Guid Id { get; set; }
    public required DateTime CreateTime { get; set; }
    public DateTime? ModifiedDate { get; set; }
    [MinLength(1)] 
    public required string Content { get; set; }
    public required Guid AuthorId { get; set; }
    public required string AuthorName { get; set; }
    public Guid? ParentId { get; set; }
}
