using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Dtos.Autores;
using WebApiAutores.Entities;
using WebApiAutores.Filters;

namespace WebApiAutores.Controllers
{
    [Route("api/autores")]
    [ApiController]
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public AutoresController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<AutorDto>>> Get()
        {
            var autoresDb = await _context.Autores.ToListAsync();
            var autoresDto = _mapper.Map<List<AutorDto>>(autoresDb);
            return autoresDto;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<AutorGetByIdDto>> GetOneById(int id)
        {
			var autorDb = await _context.Autores.Include(a => a.Books).FirstOrDefaultAsync(a => a.Id == id);
            
            if (autorDb is null)
            {
                return NotFound();
            }
            
            var autorDto = _mapper.Map<AutorGetByIdDto>(autorDb);
            
            return autorDto;
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
