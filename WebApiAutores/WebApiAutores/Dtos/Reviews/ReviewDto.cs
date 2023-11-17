using WebApiAutores.Dtos.Respuestas;

namespace WebApiAutores.Dtos.Reviews;

public class ReviewDto
{
    public int Id { get; set; }
    public Guid BookId { get; set; }
    public double Valoracion { get; set; }
    public string Comentario { get; set; }
    public Guid UsuarioId { get; set; }
    public DateTime FechaPublicacion { get; set; }
    public bool Conmentable { get; set; }
    //propiedad que traiga todas las respuestas de la review
    public ICollection<RespuestaDto> Respuestas { get; set; }
}