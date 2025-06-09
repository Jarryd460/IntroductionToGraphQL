using System.ComponentModel.DataAnnotations;

namespace IntroductionToGraphQL.Models;

public sealed class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public decimal Price { get; set; }
}
