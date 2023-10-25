using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Dtos;
using WebApiAutores.Entities;

namespace WebApiAutores.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public BooksController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<Book>>> Get()
        {
            return await _context.Books.ToListAsync();
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<BookDto>> Get(Guid id)
        {
            var book = await _context.Books
                .Include(b => b.Autor)//para que incluya el autor y se mire en la peticion es como el join de sql
                .FirstOrDefaultAsync(x => x.Id == id);

            var bookDto = _mapper.Map<BookDto>(book);

            return bookDto;
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
        
        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Put(Guid id, Book model)//actualizar un libro
        {
            var book = await _context.Books.FirstOrDefaultAsync(x => x.Id == id);
            if (book is null)
            {
                return NotFound($"No existe el libro con el id: {id}");
            }
            var autorExiste = await _context.Autores.AnyAsync(x => x.Id == model.AutorId);
            if (!autorExiste)
            {
                return BadRequest($"No existe el autor: {model.AutorId}");// el $ para inyectar el valor de la variable
            }
            book.ISBN = model.ISBN;
            book.Title = model.Title;
            book.PublicationDate = model.PublicationDate;
            book.AutorId = model.AutorId;
            await _context.SaveChangesAsync();
            return Ok($"Libro con el id: {id} actualizado correctamente");
        }
        
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var book = await _context.Books.FirstOrDefaultAsync(x => x.Id == id);
            if (book is null)
            {
                return NotFound($"No existe el libro con el id: {id}");
            }
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return Ok($"Libro con el id: {id} eliminado correctamente");
        }
    }
}
