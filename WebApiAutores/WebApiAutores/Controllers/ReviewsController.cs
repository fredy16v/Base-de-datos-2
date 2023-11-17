using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using WebApiAutores.Dtos;
using WebApiAutores.Dtos.Respuestas;
using WebApiAutores.Dtos.Reviews;
using WebApiAutores.Entities;
using WebApiAutores.Filters;

namespace WebApiAutores.Controllers
{
    [Route("api/reviews")]
    [ApiController]
    [Authorize]
    public class ReviewsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _contextAccessor;

        public ReviewsController(ApplicationDbContext context, IMapper mapper, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _contextAccessor = contextAccessor;
        }
        
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IReadOnlyList<ReviewDto>>> Get()
        {
            var reviewsDb = await _context.Reviews.ToListAsync();
            var reviewsDto = _mapper.Map<List<ReviewDto>>(reviewsDb);
            return Ok(new ResponseDto<IReadOnlyList<ReviewDto>>
            {
                Status = true,
                Data = reviewsDto
            });
        }
        
        [HttpGet("respuestas")]
        [AllowAnonymous]
        public async Task<ActionResult<IReadOnlyList<ReviewDto>>> GetWithRespuestas()
        {
            var reviewsDb = await _context.Reviews.ToListAsync();
            var reviewsDto = _mapper.Map<List<ReviewDto>>(reviewsDb);
            //que muestres las respuestas de cada review y los hijos de cada respuesta
            foreach (var reviewDto in reviewsDto)
            {
                reviewDto.Respuestas = await GetRespuestasRecursivo(reviewDto.Id);
            }
            
            return Ok(new ResponseDto<IReadOnlyList<ReviewDto>>
            {
                Status = true,
                Data = reviewsDto
            });
        }
        
        private async Task<List<RespuestaDto>> GetRespuestasRecursivo(int reviewId)
        {
            var respuestasDb = await _context.Respuestas
                .Where(r => r.ReviewId == reviewId)
                .ToListAsync();

            var respuestasDto = _mapper.Map<List<RespuestaDto>>(respuestasDb);

            foreach (var respuestaDto in respuestasDto)
            {
                respuestaDto.RespuestasHijas = await GetRespuestasRecursivo(respuestaDto.Id);
            }

            return respuestasDto;
        }

        //endpoint para obtener las reviews de un libro
        [HttpGet("book/{bookId:guid}")]
        [AllowAnonymous]
        public async Task<ActionResult<IReadOnlyList<ReviewDto>>> GetByBookId(Guid bookId)
        {
            var bookExists = await _context.Books.FirstOrDefaultAsync(a => a.Id == bookId); // arreglar esto
            if (bookExists is null)
            {
                return NotFound(new ResponseDto<IReadOnlyList<ReviewDto>>
                {
                    Status = false,
                    Message = $"No existe el libro con el id: {bookId}"
                });
            }
            
            var reviewsDb = await _context.Reviews.Where(x => x.BookId == bookId).ToListAsync();

            if (!reviewsDb.Any())
            {
                return NotFound(new ResponseDto<IReadOnlyList<ReviewDto>>
                {
                    Status = false,
                    Message = $"No existen reviews para el libro con el id: {bookId}"
                });
            }
            
            var reviewsDto = _mapper.Map<List<ReviewDto>>(reviewsDb);
            return Ok(new ResponseDto<IReadOnlyList<ReviewDto>>
            {
                Status = true,
                Data = reviewsDto
            });
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDto<ReviewDto>>> Post(ReviewCreateDto dto)
        {
            var bookExists = await _context.Books.AnyAsync(x => x.Id == dto.BookId);//arreglar esto
            if (!bookExists)
            {
                return NotFound(new ResponseDto<ReviewDto>
                {
                    Status = false,
                    Message = $"No existe el libro con el id: {dto.BookId}"
                });
            }

            var textoFiltrado = FiltroLenguajeOfensivo.ContienePalabraOfensiva(dto.Comentario);
            if(textoFiltrado is true)
            {
                return BadRequest(new ResponseDto<ReviewDto>
                {
                    Status = false,
                    Message = "El comentario contiene lenguaje ofensivo"
                });
            }

            var idClaim = _contextAccessor.HttpContext.User.Claims.Where(x => x.Type == "UserId").FirstOrDefault();
            var userId = idClaim.Value;

            var review = _mapper.Map<Review>(dto);
            review.UsuarioId = Guid.Parse(userId);
            review.FechaPublicacion = DateTime.Now;

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            var reviewDto = _mapper.Map<ReviewDto>(review);

            return Ok(new ResponseDto<ReviewDto>
            {
                Status = true,
                Message = "La review se agregó correctamente",
                Data = reviewDto
            });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ResponseDto<ReviewDto>>> Put(int id, ReviewUpdateDto dto)
        {
            var reviewDb = await _context.Reviews.FirstOrDefaultAsync(x => x.Id == id);
            if (reviewDb is null)
            {
                return NotFound(new ResponseDto<ReviewDto>
                {
                    Status = false,
                    Message = $"No existe la review con el id: {id}"
                });
            }

            var bookExists = await _context.Books.AnyAsync(x => x.Id == dto.BookId);
            if (!bookExists)
            {
                return NotFound(new ResponseDto<ReviewDto>
                {
                    Status = false,
                    Message = $"No existe el libro con el id: {dto.BookId}"
                });
            }
            
            var textoFiltrado = FiltroLenguajeOfensivo.ContienePalabraOfensiva(dto.Comentario);
            if(textoFiltrado is true)
            {
                return BadRequest(new ResponseDto<ReviewDto>
                {
                    Status = false,
                    Message = "El comentario contiene lenguaje ofensivo"
                });
            }

            var idClaim = _contextAccessor.HttpContext.User.Claims.Where(x => x.Type == "UserId").FirstOrDefault();
            var userId = idClaim.Value;

            _mapper.Map<ReviewUpdateDto, Review>(dto, reviewDb);
            reviewDb.UsuarioId = Guid.Parse(userId);

            _context.Update(reviewDb);
            await _context.SaveChangesAsync();

            var reviewDto = _mapper.Map<ReviewDto>(reviewDb);

            return Ok(new ResponseDto<ReviewDto>
            {
                Status = true,
                Message = "La review se actualizó correctamente",
                Data = reviewDto
            });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseDto<string>>> Delete(int id)
        {
            var reviewExist = await _context.Reviews.AnyAsync(x => x.Id == id);
            if (!reviewExist)
            {
                return NotFound(new ResponseDto<ReviewDto>
                {
                    Status = false,
                    Message = $"No existe la review con el id: {id}"
                });
            }

            _context.Remove(new Review() { Id = id });
            await _context.SaveChangesAsync();

            return Ok(new ResponseDto<ReviewDto>
            {
                Status = true,
                Message = "La review se eliminó correctamente"
            });
        }
    }
}