using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace WebApiAutores.Entities;

[Table("respuestas", Schema = "transacctional")]
public class Respuesta
{
    [Column("id")]
    public int Id { get; set; }
    [Column("review_id")]
    [Required]
    public int ReviewId { get; set; }
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
    public Review Review { get; set; }
    public IdentityUser User { get; set; }
    [Column("respuesta_padre")]
    public int? RespuestaPadreId { get; set; }
    [ForeignKey(nameof(RespuestaPadreId))]
    public Respuesta RespuestaPadre { get; set; }
    public ICollection<Respuesta> RespuestasHijas { get; set; }
}
