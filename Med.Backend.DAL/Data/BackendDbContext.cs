using Microsoft.EntityFrameworkCore;

namespace Med.Backend.DAL.Data;

public class BackendDbContext : DbContext
{

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

    }

    public BackendDbContext(DbContextOptions<BackendDbContext> options) : base(options)
    {
    }
}
