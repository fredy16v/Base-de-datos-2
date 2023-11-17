using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Dtos;
using WebApiAutores.Dtos.Respuestas;
using WebApiAutores.Dtos.Reviews;
using WebApiAutores.Entities;
using WebApiAutores.Filters;

namespace WebApiAutores.Controllers
{
    [Route("api/respuestas")]
    [ApiController]
    [Authorize]
    public class RespuestasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _contextAccessor;

        public RespuestasController(ApplicationDbContext context, IMapper mapper, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _contextAccessor = contextAccessor;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IReadOnlyList<RespuestaDto>>> Get()
        {
            var respuestasDb = await _context.Respuestas
                .Where(r => r.RespuestaPadre == null) // Respuestas principales
                .ToListAsync();

            var respuestasDto = _mapper.Map<List<RespuestaDto>>(respuestasDb);

            foreach (var respuestaDto in respuestasDto)
            {
                respuestaDto.RespuestasHijas = await GetRespuestasHijasRecursivo(respuestaDto.Id);
            }

            return Ok(new ResponseDto<IReadOnlyList<RespuestaDto>>
            {
                Status = true,
                Data = respuestasDto
            });
        }

        private async Task<List<RespuestaDto>> GetRespuestasHijasRecursivo(int respuestaId)
        {
            var respuestasDb = await _context.Respuestas
                .Where(r => r.RespuestaPadreId == respuestaId)
                .ToListAsync();

            var respuestasDto = _mapper.Map<List<RespuestaDto>>(respuestasDb);

            foreach (var respuestaDto in respuestasDto)
            {
                respuestaDto.RespuestasHijas = await GetRespuestasHijasRecursivo(respuestaDto.Id);
            }

            return respuestasDto;
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDto<RespuestaDto>>> Post(RespuestaCreateDto dto)
        {
            var idClaim = _contextAccessor.HttpContext.User.Claims
                .Where(x => x.Type == "UserId")
                .FirstOrDefault();
            var userId = idClaim.Value;

            // Validar que exista la review
            var reviewDb = await _context.Reviews
                .FirstOrDefaultAsync(x => x.Id == dto.ReviewId);
            if (reviewDb is null)
            {
                return NotFound(new ResponseDto<RespuestaDto>
                {
                    Status = false,
                    Message = $"No existe la review con el id: {dto.ReviewId}"
                });
            }

            if (reviewDb.Conmentable == false)
            {
                return BadRequest(new ResponseDto<RespuestaDto>
                {
                    Status = false,
                    Message = $"La review con el id: {dto.ReviewId} no permite comentarios"
                });
            }

            // Validar que exista la respuesta padre
            if (dto.RespuestaPadreId.HasValue)
            {
                var respuestaPadreDb = await _context.Respuestas
                    .FirstOrDefaultAsync(x => x.Id == dto.RespuestaPadreId);
                if (respuestaPadreDb is null)
                {
                    return NotFound(new ResponseDto<RespuestaDto>
                    {
                        Status = false,
                        Message = $"No existe la respuesta padre con el id: {dto.RespuestaPadreId}"
                    });
                }
            }

            var textoFiltrado = FiltroLenguajeOfensivo.ContienePalabraOfensiva(dto.Comentario);
            if (textoFiltrado is true)
            {
                return BadRequest(new ResponseDto<RespuestaDto>
                {
                    Status = false,
                    Message = "El comentario contiene lenguaje ofensivo"
                });
            }

            var respuestaDb = _mapper.Map<Respuesta>(dto);
            respuestaDb.UsuarioId = Guid.Parse(userId);
            respuestaDb.FechaPublicacion = DateTime.UtcNow;

            _context.Add(respuestaDb);
            await _context.SaveChangesAsync();

            var respuestaDto = _mapper.Map<RespuestaDto>(respuestaDb);

            return Ok(new ResponseDto<RespuestaDto>
            {
                Status = true,
                Data = respuestaDto
            });
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ResponseDto<RespuestaDto>>> Put(int id, RespuestaUpdateDto dto)
        {
            var respuestaDb = await _context.Respuestas.FirstOrDefaultAsync(x => x.Id == id);
            if (respuestaDb is null)
            {
                return NotFound(new ResponseDto<RespuestaDto>
                {
                    Status = false,
                    Message = $"No existe la respuesta con el id: {id}"
                });
            }
            
            var textoFiltrado = FiltroLenguajeOfensivo.ContienePalabraOfensiva(dto.Comentario);
            if (textoFiltrado is true)
            {
                return BadRequest(new ResponseDto<RespuestaDto>
                {
                    Status = false,
                    Message = "El comentario contiene lenguaje ofensivo"
                });
            }

            var idClaim = _contextAccessor.HttpContext.User.Claims
                .Where(x => x.Type == "UserId")
                .FirstOrDefault();
            var userId = idClaim.Value;

            if (respuestaDb.UsuarioId.ToString() != userId)
            {
                return BadRequest(new ResponseDto<RespuestaDto>
                {
                    Status = false,
                    Message = $"El usuario con el id: {userId} no es el autor de la respuesta"
                });
            }

            respuestaDb.Comentario = dto.Comentario;
            await _context.SaveChangesAsync();

            var respuestaDto = _mapper.Map<RespuestaDto>(respuestaDb);

            return Ok(new ResponseDto<RespuestaDto>
            {
                Status = true,
                Data = respuestaDto
            });
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ResponseDto<string>>> Delete(int id)
        {
            var respuestaDb = await _context.Respuestas.FirstOrDefaultAsync(x => x.Id == id);
            if (respuestaDb is null)
            {
                return NotFound(new ResponseDto<string>
                {
                    Status = false,
                    Message = $"No existe la respuesta con el id: {id}"
                });
            }

            var idClaim = _contextAccessor.HttpContext.User.Claims
                .Where(x => x.Type == "UserId")
                .FirstOrDefault();
            var userId = idClaim.Value;

            if (respuestaDb.UsuarioId.ToString() != userId)
            {
                return BadRequest(new ResponseDto<string>
                {
                    Status = false,
                    Message = $"El usuario con el id: {userId} no es el autor de la respuesta"
                });
            }

            // Eliminar respuesta y respuestas hijas recursivamente
            await DeleteRespuestaYHijasRecursivo(respuestaDb);

            return Ok(new ResponseDto<string>
            {
                Status = true,
                Message = "La respuesta y sus respuestas hijas se eliminaron correctamente"
            });
        }

        private async Task DeleteRespuestaYHijasRecursivo(Respuesta respuesta)
        {
            _context.Remove(respuesta);
            await _context.SaveChangesAsync();

            // Eliminar respuestas hijas recursivamente
            var respuestasHijas = await _context.Respuestas
                .Where(r => r.RespuestaPadreId == respuesta.Id)
                .ToListAsync();

            foreach (var respuestaHija in respuestasHijas)
            {
                await DeleteRespuestaYHijasRecursivo(respuestaHija);
            }
        }
    }
}