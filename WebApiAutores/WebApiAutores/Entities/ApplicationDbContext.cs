using Microsoft.EntityFrameworkCore;

namespace WebApiAutores.Entities;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder) //usar el api fluent para configurar la base de datos
    {
        base.OnModelCreating(builder);
        
        // para afectar la propiedad de la tabla de books y que sea unica
        builder.Entity<Book>()
            .HasIndex(x => x.ISBN)
            .IsUnique(true);
    }

    public DbSet<Autor> Autores { get; set; }
    public DbSet<Book> Books { get; set; }
}