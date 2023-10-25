using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using WebApiAutores.Dtos;
using WebApiAutores.Entities;

namespace WebApiAutores.Helpers;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        MapsForBooks();
    }

    void MapsForBooks()
    {
        //CreateMap<BookDto, Book>().ReverseMap();
        CreateMap<Book, BookDto>().ForPath(dest => dest.AutorNombre, opt => opt.MapFrom(src => src.Autor.Name));
        
        CreateMap<BookCreateDto, Book>();
    }
}