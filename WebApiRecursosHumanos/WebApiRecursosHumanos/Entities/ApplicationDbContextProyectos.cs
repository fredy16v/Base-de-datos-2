using Microsoft.EntityFrameworkCore;

namespace WebApiRecursosHumanos.Entities;

public class ApplicationDbContextProyectos : DbContext
{
    public ApplicationDbContextProyectos(DbContextOptions<ApplicationDbContextProyectos> options) : base(options)
    {
    }
    
    public DbSet<Proyecto> Proyectos { get; set; }
    public DbSet<ProyectoEmpleado> ProyectoEmpleados { get; set; }
}