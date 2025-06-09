using IntroductionToGraphQL.Infrastructure;

namespace IntroductionToGraphQL.Models;

public sealed class Mutation
{
    public async Task<Book> AddBookAsync(Book book, [Service] BookContext context)
    {
        context.Books.Add(book);
        await context.SaveChangesAsync();
        return book;
    }
}
