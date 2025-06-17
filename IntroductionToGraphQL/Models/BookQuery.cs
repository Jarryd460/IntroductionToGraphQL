using IntroductionToGraphQL.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace IntroductionToGraphQL.Models;

[QueryType]
public sealed class BookQuery
{
    // UseProjection allows us to fetch related objects along with books without specifying .Includes(book => book.Author). It is implicitly included by HotChocolate if related objects are requested
    // This is more performant as related objects are only requested and loaded if found to be in the GraphQL query
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    [GraphQLName("books")]
    public IQueryable<Book> GetBooksAsync([Service] BookContext context) => context.Books.AsNoTracking();

    [GraphQLName("book")]
    // Specify [GraphQLType(typeof(IdType))] so that GraphQL uses type ID under the hood
    public async Task<Book?> GetBookAsync([GraphQLType(typeof(NonNullType<IdType>))] int id, [Service] BookContext context, CancellationToken cancellationToken) => await context.Books.AsNoTracking().SingleOrDefaultAsync(book => book.Id == id, cancellationToken).ConfigureAwait(false);
}
