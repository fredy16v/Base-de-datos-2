using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiRecursosHumanos.Entities;

namespace WebApiRecursosHumanos.Controllers
{
    [Route("api/empleados")]
    [ApiController]
    public class EmpleadosController : ControllerBase
    {
        private readonly ApplicationDbContextRecursosHumanos _context;
        private readonly ApplicationDbContextProyectos _contextProyectos;

        public EmpleadosController(ApplicationDbContextRecursosHumanos context, ApplicationDbContextProyectos contextProyectos)
        {
            _context = context;
            _contextProyectos = contextProyectos;
        }

        [HttpGet]
        public async Task<ActionResult<List<Empleado>>> Get()
        {
            return await _context.Empleados.ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Empleado>> GetById(int id)
        {
            var empleado = await _context.Empleados.FirstOrDefaultAsync(x => x.Id == id);

            if (empleado is null)
            {
                return NotFound();
            }
            
            return empleado;
        }

        [HttpPost]
        public async Task<ActionResult> Post(Empleado modelo)
        {
            _contextProyectos.Add(modelo);
            //await _contextProyectos.SaveChangesAsync();
            
            _context.Add(modelo);
            await _context.SaveChangesAsync();
            return Ok();
        }
        
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, Empleado modelo)
        {
            var empleado = await _context.Empleados.FirstOrDefaultAsync(x => x.Id == id);
            if (empleado is null)
            {
                return NotFound("Empleado no encontrado");
            }

            empleado.Nombre = modelo.Nombre;
            empleado.Apellido = modelo.Apellido;
            empleado.FechaContratacion = modelo.FechaContratacion;
            empleado.Profesion = modelo.Profesion;
            empleado.Direccion = modelo.Direccion;
            empleado.CorreoElectronico = modelo.CorreoElectronico;
            empleado.NumeroTelefono = modelo.NumeroTelefono;
            
            _context.Update(empleado);
            await _context.SaveChangesAsync();
            return Ok();
        }
        
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var empleado = await _context.Empleados.FirstOrDefaultAsync(x => x.Id == id);
            if (empleado is null)
            {
                return NotFound("Empleado no encontrado");
            }

            _context.Remove(empleado);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
