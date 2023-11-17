using System.ComponentModel.DataAnnotations;

namespace WebApiAutores.Dtos.Reviews;

public class ReviewCreateDto
{
    [Display(Name = "Libro")]
    [Required(ErrorMessage = "El campo {0} es requerido")]
    public Guid BookId { get; set; }
    [Display(Name = "Valoración")]
    [Required(ErrorMessage = "El campo {0} es requerido")]
    public double Valoracion { get; set; }
    [Display(Name = "Comentario")]
    [Required]
    [StringLength(1000, ErrorMessage = "El campo {0} no debe tener más de {1} caracteres")]
    public string Comentario { get; set; }
    [Display(Name = "Fecha de publicación")]
    [DataType(DataType.Date)]
    public DateTime FechaPublicacion { get; set; }
    [Display(Name = "¿Es comentable?")]
    public bool Conmentable { get; set; } = true;
}