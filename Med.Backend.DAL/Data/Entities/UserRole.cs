﻿using Microsoft.AspNetCore.Identity;

namespace Med.Backend.DAL.Data.Entities;
public class UserRole : IdentityUserRole<Guid>
{
    public virtual User User { get; set; }
    public virtual Role Role { get; set; }
}