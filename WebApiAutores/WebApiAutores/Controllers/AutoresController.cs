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
        
        [HttpPost]
        public async Task<ActionResult> Post(Autor modelo)
        {
            _context.Add(modelo);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
