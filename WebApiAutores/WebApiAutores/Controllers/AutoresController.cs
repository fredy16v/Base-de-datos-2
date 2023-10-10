using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Entities;

namespace WebApiAutores.Controllers
{
    [Route("api/autores")]
    [ApiController]
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AutoresController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public async Task<ActionResult<List<Autor>>> Get()
        {
            return await _context.Autores.ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Autor>> GetOneById(int id)
        {
            return await _context.Autores.FirstOrDefaultAsync(a => a.Id == id);
        }
        
        [HttpPost]
        public async Task<ActionResult> Post(Autor modelo)
        {
            _context.Add(modelo);
            await _context.SaveChangesAsync();
            return Ok();
        }
        
        [HttpPut("{id:int}")] //para actualizar y como ocupamos el id, lo ponemos en la ruta
        public async Task<ActionResult> Put(int id, Autor modelo)
        {
            var autor = await _context.Autores.FirstOrDefaultAsync(a => a.Id == id);
            if (autor is null)
            {
                return NotFound("Autor no encontrado");
            }
            
            autor.Name = modelo.Name;
            _context.Update(autor);
            await _context.SaveChangesAsync();
            
            return Ok();
        }
        
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var autor = await _context.Autores.FirstOrDefaultAsync(a => a.Id == id);
            if (autor is null)
            {
                return NotFound("Autor no encontrado");
            }
            
            _context.Remove(autor);
            await _context.SaveChangesAsync();
            
            return Ok();
        }
    }
}
