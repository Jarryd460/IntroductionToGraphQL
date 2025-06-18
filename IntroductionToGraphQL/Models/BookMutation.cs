using IntroductionToGraphQL.Infrastructure;
using IntroductionToGraphQL.Models.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace IntroductionToGraphQL.Models;

[MutationType]
public sealed class BookMutation
{
    [Error(typeof(BookWithTitleExistsException))]
    public async Task<Book> AddBookAsync(Book book, [Service] BookContext context)
    {
        var booksWithSameTitle = await context.Books.AsNoTracking().Where(b => b.Title == book.Title).ToListAsync().ConfigureAwait(false);

        if (booksWithSameTitle.Any())
        {
            throw new BookWithTitleExistsException(book.Title);
        }

        await context.Books.AddAsync(book).ConfigureAwait(false);
        await context.SaveChangesAsync().ConfigureAwait(false);

        return book;
    }

    [Error(typeof(BookNotFoundException))]
    [Error(typeof(BookWithTitleExistsException))]
    public async Task<Book> UpdateBookAsync(int id, Book updatedBook, [Service] BookContext context, CancellationToken cancellationToken)
    {
        var book = await context.Books.FindAsync([id], cancellationToken).ConfigureAwait(false);

        if (book is null)
        {
            throw new BookNotFoundException(id);
        }

        var booksWithSameTitle = await context.Books.AsNoTracking().Where(b => b.Id != book.Id && b.Title == book.Title).ToListAsync().ConfigureAwait(false);

        if (booksWithSameTitle.Any())
        {
            throw new BookWithTitleExistsException(book.Title);
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
