using Microsoft.EntityFrameworkCore;

namespace WebApiRecursosHumanos.Entities;

public class ApplicationDbContextRecursosHumanos : DbContext
{
    public ApplicationDbContextRecursosHumanos(DbContextOptions<ApplicationDbContextRecursosHumanos> options) : base(options)
    {
    }
    
    public DbSet<Empleado> Empleados { get; set; }
}