namespace WebApiAutores.Dtos.Reviews;

public class BooksReviewGetById : BookDto
{
    public virtual IEnumerable<ReviewDto> Reviews { get; set; }
}