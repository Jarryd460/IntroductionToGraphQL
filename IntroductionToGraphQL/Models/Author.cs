using IntroductionToGraphQL.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace IntroductionToGraphQL.Models;

public sealed class Author
{
    // Specify [GraphQLType(typeof(NonNullType<IdType>))] so that GraphQL uses type ID under the hood. See BookType for another way to do this for bigger projects
    [GraphQLType(typeof(NonNullType<IdType>))]
    public int Id { get; set; }
    public string Name { get; set; } = default!;

    // Specify resolver for Books property to fetch related books for an author. See BookType for another way to do this for bigger projects
    [GraphQLName("books")]
    public IQueryable<Book> GetBooksAsync([Parent] Author author, BookContext context, CancellationToken cancellation)
    {
        return context.Books.Where(book => book.AuthorId == author.Id).AsNoTracking();
    }
}
