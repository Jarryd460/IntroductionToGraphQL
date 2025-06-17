using IntroductionToGraphQL.Infrastructure;

namespace IntroductionToGraphQL.Models;

[MutationType]
public sealed class BookMutation
{
    public async Task<Book> AddBookAsync(Book book, [Service] BookContext context)
    {
        await context.Books.AddAsync(book).ConfigureAwait(false);
        await context.SaveChangesAsync().ConfigureAwait(false);

        return book;
    }

    public async Task<Book> UpdateBookAsync(int id, Book updatedBook, [Service] BookContext context, CancellationToken cancellationToken)
    {
        var book = await context.Books.FindAsync([id], cancellationToken).ConfigureAwait(false);

        if (book is null)
        {
            throw new Exception($"Book with Id: {id} not found");
        }

        book.Title = updatedBook.Title;
        book.Price = updatedBook.Price;
        book.AuthorId = updatedBook.AuthorId;
        book.Author = updatedBook.Author;        

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return book;
    }

    public async Task<bool> DeleteBookAsync(int id, [Service] BookContext context, CancellationToken cancellationToken)
    {
        var book = await context.Books.FindAsync([id], cancellationToken).ConfigureAwait(false);

        if (book is null)
        {
            return false;
        }

        context.Books.Remove(book);
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return true;
    }
}
