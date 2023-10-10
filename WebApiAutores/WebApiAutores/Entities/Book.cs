using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Build.Evaluation;

namespace WebApiAutores.Entities;

[Table("books")]
public class Book
{
    public Guid Id { get; set; }
    public string ISBN { get; set; }
    public string Title { get; set; }
    public DateTime PublicationDate { get; set; }
    public int AutorId { get; set; }
    public Autor Autor { get; set; }
}