using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Entities;

namespace WebApiAutores.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        
        public BooksController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Book>> Get(Guid id)
        {
            return await _context.Books
                .Include(b => b.Autor)//para que incluya el autor y se mire en la peticion es como el join de sql
                .FirstOrDefaultAsync(x => x.Id == id);
        }
        
        [HttpPost]
        public async Task<ActionResult> Post(Book model)//crear un libro
        {
            var autorExiste = await _context.Autores.AnyAsync(x => x.Id == model.AutorId);
            if (!autorExiste)
            {
                return BadRequest($"No existe el autor: {model.AutorId}");// el $ para inyectar el valor de la variable
            }
            _context.Books.Add(model);
            await _context.SaveChangesAsync();
            return Ok("Libro creado correctamente");
        }
    }
}
