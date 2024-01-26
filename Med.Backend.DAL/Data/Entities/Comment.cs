using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Med.Backend.DAL.Data.Entities;

public class Comment
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreateTime { get; set; } = DateTime.UtcNow;
    public DateTime? ModifiedTime { get; set; }
    public string Content { get; set; }
    public Guid AuthorId { get; set; }
    public string AuthorName { get; set; }
    public Guid? ParentCommentId { get; set; }
}
