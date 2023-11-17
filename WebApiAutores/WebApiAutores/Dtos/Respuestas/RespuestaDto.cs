namespace WebApiAutores.Dtos.Respuestas;

public class RespuestaDto
{
    public int Id { get; set; }
    public int ReviewId { get; set; }
    public string Comentario { get; set; }
    public Guid UsuarioId { get; set; }
    public DateTime FechaPublicacion { get; set; }
    public string Review { get; set; }
    public int? RespuestaPadreId { get; set; }
    public ICollection<RespuestaDto> RespuestasHijas { get; set; }
}