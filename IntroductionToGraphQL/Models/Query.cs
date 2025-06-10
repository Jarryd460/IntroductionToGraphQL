using IntroductionToGraphQL.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace IntroductionToGraphQL.Models;

public sealed class Query
{
    // UseProjection allows us to fetch related objects along with books without specifying .Includes(book => book.Author)
    // This is more performant as the GraphQL query is applied on the database level and not on the application level as it would be if we included the above code
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Book> GetBooks([Service] BookContext context) => context.Books.AsNoTracking();
}
