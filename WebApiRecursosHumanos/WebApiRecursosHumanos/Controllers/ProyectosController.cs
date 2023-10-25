using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiRecursosHumanos.Entities;

namespace WebApiRecursosHumanos.Controllers
{
    [Route("api/proyectos")]
    [ApiController]
    public class ProyectosController : ControllerBase
    {
        private readonly ApplicationDbContextProyectos _context;
        private readonly ApplicationDbContextRecursosHumanos _contextEmpleados;

        public ProyectosController(ApplicationDbContextProyectos context,
            ApplicationDbContextRecursosHumanos contextEmpleados)
        {
            _context = context;
            _contextEmpleados = contextEmpleados;
        }

        [HttpGet]
        public async Task<ActionResult<List<Proyecto>>> Get()
        {
            //retornar todos los proyectos y los empleados que estan en cada proyecto con los nombres de esos empleados
            return await _context.Proyectos.Include(p => p.ProyectoEmpleados).ThenInclude(pe => pe.Empleado)
                .ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Proyecto>> GetById(int id)
        {
            //retornar un proyecto y los empleados que estan en ese proyecto con los nombres de esos empleados 
            var proyecto = await _context.Proyectos.Include(p => p.ProyectoEmpleados).ThenInclude(pe => pe.Empleado)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (proyecto is null)
            {
                return NotFound();
            }
            
            return proyecto;
        }

        // Método para crear un nuevo proyecto
        [HttpPost]
        public async Task<ActionResult> CreateProject(Proyecto modelo)
        {
            _context.Proyectos.Add(modelo);
            await _context.SaveChangesAsync();
            return Ok();
        }

        // Método para agregar empleados a un proyecto existente
        [HttpPost("{projectId}/empleados")]
        public async Task<ActionResult> AddEmployeesToProject(int projectId, List<int> EmployeesIds)
        {
            var project = await _context.Proyectos.Include(p => p.ProyectoEmpleados)
                .FirstOrDefaultAsync(p => p.Id == projectId);
            if (project is null)
            {
                return NotFound("Proyecto no encontrado");
            }

            foreach (var employeeId in EmployeesIds)
            {
                var employee = await _contextEmpleados.Empleados.FirstOrDefaultAsync(e => e.Id == employeeId);
                if (employee is null)
                {
                    return NotFound($"No se encontró el empleado con el ID {employeeId}");
                }

                project.ProyectoEmpleados.Add(new ProyectoEmpleado
                    { EmpleadoId = employee.Id, ProyectoId = projectId });
            }

            await _context.SaveChangesAsync();
            return Ok();
        }


        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, Proyecto modelo)
        {
            var proyecto = await _context.Proyectos.FirstOrDefaultAsync(x => x.Id == id);
            if (proyecto is null)
            {
                return NotFound("Proyecto no encontrado");
            }

            proyecto.NombreProyecto = modelo.NombreProyecto;
            proyecto.Descripcion = modelo.Descripcion;
            proyecto.FechaInicio = modelo.FechaInicio;
            proyecto.FechaFin = modelo.FechaFin;
            proyecto.ProyectoEmpleados = modelo.ProyectoEmpleados;

            _context.Update(proyecto);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var proyecto = await _context.Proyectos.FirstOrDefaultAsync(a => a.Id == id);

            if (proyecto is null)
            {
                return NotFound("Poyecto no encontrado");
            }

            _context.Remove(proyecto);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}