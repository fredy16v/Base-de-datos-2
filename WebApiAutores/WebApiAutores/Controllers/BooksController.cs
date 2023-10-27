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
        public async Task<ActionResult<IReadOnlyList<BookDto>>> Get()
        {
            var booksDb = await _context.Books.Include(b => b.Autor).ToListAsync();
            
            var booksDto = _mapper.Map<List<BookDto>>(booksDb);

            return booksDto;
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<BookDto>> GetById(Guid id)
        {
            var book = await _context.Books
                .Include(b => b.Autor)//para que incluya el autor y se mire en la peticion es como el join de sql
                .FirstOrDefaultAsync(x => x.Id == id);

            var bookDto = _mapper.Map<BookDto>(book);

            return bookDto;
        }
        
        [HttpPost]
        public async Task<ActionResult> Post(BookCreateDto dto)//crear un libro
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Los datos del libro son incorrectos");
            }
            var autorExiste = await _context.Autores.AnyAsync(x => x.Id == dto.AutorId);
            if (!autorExiste)
            {
                return BadRequest($"No existe el autor: {dto.AutorId}");// el $ para inyectar el valor de la variable
            }

            var book = _mapper.Map<Book>(dto);
            
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return Ok("Libro creado correctamente");
        }
        
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(Guid id, BookUpdateDto dto)//actualizar un libro
        {
            var bookDb = await _context.Books.FirstOrDefaultAsync(x => x.Id == id);
            if (bookDb is null)
            {
                return NotFound($"No existe el libro con el id: {id}");
            }
            
            var autorExiste = await _context.Autores.AnyAsync(x => x.Id == dto.AutorId);
            if (!autorExiste)
            {
                return BadRequest($"No existe el autor: {dto.AutorId}");// el $ para inyectar el valor de la variable
            }

            _mapper.Map<BookUpdateDto, Book>(dto, bookDb);
            
            _context.Update(bookDb);
            await _context.SaveChangesAsync();
            return Ok($"Libro con el id: {id} actualizado correctamente");
        }
        
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var bookExist = await _context.Books.AnyAsync(x => x.Id == id);
            if (!bookExist)
            {
                return NotFound($"No existe el libro con el id: {id}");
            }
            _context.Remove(new Book() {Id = id});
            await _context.SaveChangesAsync();
            return Ok(new {msg = $"Libro con el id: {id} se elimino correctamente"});
        }
    }
}
