using IntroductionToGraphQL.Infrastructure;

namespace IntroductionToGraphQL.Models;

public sealed class Query
{
    [UseFiltering]
    [UseSorting]
    public IQueryable<Book> GetBooks([Service] BookContext context) => context.Books;
}
