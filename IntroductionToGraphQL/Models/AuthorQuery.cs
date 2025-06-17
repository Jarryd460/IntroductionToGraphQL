using IntroductionToGraphQL.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace IntroductionToGraphQL.Models;

[QueryType]
public sealed class AuthorQuery
{
    // UseProjection allows us to fetch related objects along with authors without specifying .Includes(author => author.Books). It is implicitly included by HotChocolate if related objects are requested
    // This is more performant as related objects are only requested and loaded if found to be in the GraphQL query
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    [GraphQLName("authors")]
    public IQueryable<Author> GetAuthorsAsync([Service] BookContext context) => context.Authors.AsNoTracking();

    [GraphQLName("author")]
    // Specify [GraphQLType(typeof(IdType))] so that GraphQL uses type ID under the hood
    public async Task<Author?> GetAuthorAsync([GraphQLType(typeof(NonNullType<IdType>))] int id, [Service] BookContext context, CancellationToken cancellationToken) => await context.Authors.AsNoTracking().SingleOrDefaultAsync(author => author.Id == id, cancellationToken).ConfigureAwait(false);
}
