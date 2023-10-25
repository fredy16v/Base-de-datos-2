using Microsoft.EntityFrameworkCore;

namespace WebApiRecursosHumanos.Entities;

public class RecursosHumanosContext : DbContext
{
    public RecursosHumanosContext(DbContextOptions<RecursosHumanosContext> options) : base(options)
    {
    }
    
    public DbSet<Empleado> Empleados { get; set; }
}

public class ProyectosContext : DbContext
{
    public ProyectosContext(DbContextOptions<ProyectosContext> options) : base(options)
    {
    }
    public DbSet<Proyecto> Proyectos { get; set; }
}