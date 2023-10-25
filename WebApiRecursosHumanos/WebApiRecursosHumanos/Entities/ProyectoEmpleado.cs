using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;

namespace WebApiRecursosHumanos.Entities;

public class ProyectoEmpleado
{
    [Key]
    public int Id { get; set; }
    public int? ProyectoId { get; set; }
    [ForeignKey("ProyectoId")]
    public Proyecto Proyecto { get; set; }
    public int? EmpleadoId { get; set; }
    [ForeignKey("EmpleadoId")]
    public Empleado Empleado { get; set; }
}