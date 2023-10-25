using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiRecursosHumanos.Entities;

[Table("empleados")]
public class Empleado
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    [Required]
    [MaxLength(50)]
    [Column("nombre")]
    public string Nombre { get; set; }
    [Required]
    [MaxLength(50)]
    [Column("apellido")]
    public string Apellido { get; set; }
    [Required]
    [DataType(DataType.Date)]
    [Column("fecha_contratacion")]
    public DateTime FechaContratacion { get; set; }
    [Required]
    [MaxLength(50)]
    [Column("profecion")]
    public string Profecion { get; set; }
    [Required]
    [MaxLength(100)]
    [Column("direccion")]
    public string Direccion { get; set; }
    [Required]
    [EmailAddress]
    [Column("correo")]
    public string CorreoElectronico { get; set; }
    [Required]
    [Column("numero_telefono")]
    public int NumeroTelefono { get; set; }

    // otras propiedades según sea necesario

    // Relaciones
    [Column("proyecto_id")]
    public int ProyectoId { get; set; }
    [ForeignKey(nameof(ProyectoId))]
    public virtual Proyecto Proyecto { get; set; }
}