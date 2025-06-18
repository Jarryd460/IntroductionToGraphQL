using IntroductionToGraphQL.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace IntroductionToGraphQL.Models;

[QueryType]
public sealed class AuthorQuery
{
    // UseProjection allows us to fetch related objects along with authors without specifying .Includes(author => author.Books). It is implicitly included by HotChocolate if related objects are requested
    // This is more performant as related objects are only requested and loaded if found to be in the GraphQL query
    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    [GraphQLName("authors")]
    public IQueryable<Author> GetAuthorsAsync([Service] BookContext context) => context.Authors.AsNoTracking();

    [UseSingleOrDefault]
    [UseProjection]
    [GraphQLName("author")]
    // Specify [GraphQLType(typeof(IdType))] so that GraphQL uses type ID under the hood
    public IQueryable<Author> GetAuthorAsync([GraphQLType(typeof(NonNullType<IdType>))] int id, [Service] BookContext context, CancellationToken cancellationToken) => context.Authors.AsNoTracking().Where(author => author.Id == id);
}
