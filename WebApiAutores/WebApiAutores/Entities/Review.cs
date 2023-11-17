using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace WebApiAutores.Entities;

[Table("reviews", Schema = "transacctional")]
public class Review
{
    [Column("id")]
    public int Id { get; set; }
    [Column("book_id")]
    [Required]
    public Guid BookId { get; set; }
    [Column("valoracion")]
    [Required]
    public double Valoracion { get; set; }
    [Column("comentario")]
    [Required]
    [StringLength(1000)]
    public string Comentario { get; set; }
    [Column("usuario_id")]
    [Required]
    public Guid UsuarioId { get; set; }
    [Column("fecha_publicacion")]
    [Required]
    public DateTime FechaPublicacion { get; set; } = DateTime.UtcNow;
    [Column("conmentable")]
    public bool Conmentable { get; set; } = true;
    
    public Book Book { get; set; }
    
    public IdentityUser User { get; set; }
    public ICollection<Respuesta> Respuestas { get; set; }
}