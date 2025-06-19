namespace IntroductionToGraphQL.Models;

public sealed class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public int AuthorId { get; set; }
    public Author Author { get; set; } = default!;
    public decimal Price { get; set; }
    public string EmailAddress { get; set; } = default!;
}
