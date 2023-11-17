using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using WebApiAutores.Dtos;
using WebApiAutores.Dtos.Autores;
using WebApiAutores.Dtos.Respuestas;
using WebApiAutores.Dtos.Reviews;
using WebApiAutores.Entities;

namespace WebApiAutores.Helpers;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        MapsForBooks();
        MapsForAutores();
        MapsForReviews();
        MapsForRespuestas();
    }

    private void MapsForBooks()
    {
        //CreateMap<BookDto, Book>().ReverseMap();
        CreateMap<Book, BookDto>().ForPath(dest => dest.AutorNombre, opt => opt.MapFrom(src => src.Autor.Name));
        
        CreateMap<BookCreateDto, Book>();
    }
    
    private void MapsForAutores()
    {
        CreateMap<Autor, AutorDto>();
        CreateMap<AutorCreateDto, Autor>();
        CreateMap<Autor, AutorCreateDto>();
        CreateMap<Autor, AutorGetByIdDto>();
    }
    
    private void MapsForReviews()
    {
        CreateMap<Review, ReviewDto>()
            .ForMember(dest => dest.UsuarioId, opt => opt.MapFrom(src => src.UsuarioId.ToString())); // O cualquier otra lógica de mapeo necesaria
        
        CreateMap<ReviewCreateDto, Review>();
        CreateMap<Review, ReviewCreateDto>();
        CreateMap<Review, BooksReviewGetById>();
    }

    private void MapsForRespuestas()
    {
        /*CreateMap<Respuesta, RespuestaDto>()
            .ForMember(dest => dest.RespuestaPadre, opt => opt.MapFrom(src => 
                src.RespuestaPadre.HasValue
                    ? new RespuestaPadreDto { Id = src.RespuestaPadre.Value }
                    : null
            ));*/

        CreateMap<Respuesta, RespuestaDto>()
            .ForMember(dest => dest.UsuarioId, opt => opt.MapFrom(src => src.UsuarioId.ToString())); // O cualquier otra lógica de mapeo necesaria
        CreateMap<RespuestaCreateDto, Respuesta>();
        CreateMap<Respuesta, RespuestaCreateDto>();
        // map para el dto de RespuestaUpdateDto
        CreateMap<Respuesta, RespuestaUpdateDto>();
    }
}