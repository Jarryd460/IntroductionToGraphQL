using HotChocolate.Subscriptions;
using IntroductionToGraphQL.Infrastructure;
using IntroductionToGraphQL.Models.Exceptions;
using IntroductionToGraphQL.Models.Subscriptions;
using Microsoft.EntityFrameworkCore;

namespace IntroductionToGraphQL.Models;

[MutationType]
public sealed class BookMutation
{
    [Error(typeof(BookWithTitleExistsException))]
    public async Task<Book> AddBookAsync(Book book, [Service] BookContext context, ITopicEventSender sender, CancellationToken cancellationToken)
    {
        var booksWithSameTitle = await context.Books.AsNoTracking().Where(b => b.Title == book.Title).ToListAsync(cancellationToken).ConfigureAwait(false);

        if (booksWithSameTitle.Any())
        {
            throw new BookWithTitleExistsException(book.Title);
        }

        await context.Books.AddAsync(book, cancellationToken).ConfigureAwait(false);
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        await sender.SendAsync(nameof(BookSubscription.BookAdded), book, cancellationToken);

        return book;
    }

    [Error(typeof(BookNotFoundException))]
    [Error(typeof(BookWithTitleExistsException))]
    public async Task<Book> UpdateBookAsync(int id, Book updatedBook, [Service] BookContext context, ITopicEventSender sender, CancellationToken cancellationToken)
    {
        var book = await context.Books.FindAsync([id], cancellationToken).ConfigureAwait(false);

        if (book is null)
        {
            throw new BookNotFoundException(id);
        }

        var booksWithSameTitle = await context.Books.AsNoTracking().Where(b => b.Id != book.Id && b.Title == book.Title).ToListAsync(cancellationToken).ConfigureAwait(false);

        if (booksWithSameTitle.Any())
        {
            throw new BookWithTitleExistsException(book.Title);
        }

        book.Title = updatedBook.Title;
        book.Price = updatedBook.Price;
        book.AuthorId = updatedBook.AuthorId;     

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        await sender.SendAsync(book.AuthorId.ToString(), book, cancellationToken);

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
