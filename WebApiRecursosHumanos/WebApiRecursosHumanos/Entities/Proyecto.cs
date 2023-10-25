using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiRecursosHumanos.Entities;

[Table("proyectos")]
public class Proyecto
{
    [Column("id")]
    public int Id { get; set; }
    [Required]
    [MaxLength(100)]
    [Column("nombre_proyecto")]
    public string NombreProyecto { get; set; }
    [Required]
    [MaxLength(500)]
    [Column("descripcion")]
    public string Descripcion { get; set; }
    [Required]
    [DataType(DataType.Date)]
    [Column("fecha_inicio")]
    public DateTime FechaInicio { get; set; }
    [Required]
    [DataType(DataType.Date)]
    [Column("fecha_fin")]
    public DateTime FechaFin { get; set; }
    
    public virtual ICollection<ProyectoEmpleado>? ProyectoEmpleados { get; set; }
}