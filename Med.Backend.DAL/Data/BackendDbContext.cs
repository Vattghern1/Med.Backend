using Med.Backend.DAL.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Med.Backend.DAL.Data;

public class BackendDbContext : IdentityDbContext<User, Role, Guid, IdentityUserClaim<Guid>,
    UserRole, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
{
    public override DbSet<User> Users { get; set; }
    public override DbSet<Role> Roles { get; set; }
    public override DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Speciality> Specialities { get; set; }
    public DbSet<Consultation> Consultations { get; set; }
    public DbSet<Inspection> Inspections { get; set; }
    public DbSet<Diagnos> Diagnos { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Icd10> Icd10s { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Role>(o => {
            //   o.ToTable("Roles");
        });

        modelBuilder.Entity<UserRole>(o => {
            // o.ToTable("UserRoles");
            o.HasOne(x => x.Role)
                .WithMany(x => x.Users)
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
            o.HasOne(x => x.User)
                .WithMany(x => x.Roles)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    public DbSet<Device> Devices { get; set; } = null!;

    public BackendDbContext(DbContextOptions<BackendDbContext> options) : base(options) 
    {


    }
}
