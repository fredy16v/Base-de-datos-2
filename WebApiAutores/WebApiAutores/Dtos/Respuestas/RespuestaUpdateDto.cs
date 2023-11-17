using System.ComponentModel.DataAnnotations;

namespace WebApiAutores.Dtos.Respuestas;

public class RespuestaUpdateDto
{
    [Display(Name = "Comentario")]
    [Required]
    [StringLength(1000, ErrorMessage = "El campo {0} debe tener {1} caracteres o menos.")]
    public string Comentario { get; set; }
}