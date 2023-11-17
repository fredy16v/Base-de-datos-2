using System.ComponentModel.DataAnnotations;
using WebApiAutores.Entities;

namespace WebApiAutores.Dtos.Respuestas;

public class RespuestaCreateDto
{
    [Display(Name = "Comentario")]
    [StringLength(1000, ErrorMessage = "El campo {0} no debe tener más de {1} caracteres")]
    [Required(ErrorMessage = "El campo {0} es requerido")]
    public string Comentario { get; set; }
    [Display(Name = "Review")]
    [Required(ErrorMessage = "El campo {0} es requerido")]
    public int ReviewId { get; set; }
    [Display(Name = "Respuesta Padre")]
    public int? RespuestaPadreId { get; set; }
    [Display(Name = "Fecha de Publicación")]
    [DataType(DataType.Date)]
    public DateTime FechaPublicacion { get; set; }
}