using IntroductionToGraphQL.Infrastructure;
using IntroductionToGraphQL.Models.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace IntroductionToGraphQL.Models;

[MutationType]
public sealed class AuthorMutation
{
    [Error(typeof(AuthorWithNameExistsException))]
    public async Task<Author> AddAuthorAsync(Author author, [Service] BookContext context, CancellationToken cancellationToken)
    {
        var authorsWithSameName = await context.Authors.AsNoTracking().Where(a => a.Name == author.Name).ToListAsync().ConfigureAwait(false);

        if (authorsWithSameName.Any())
        {
            throw new AuthorWithNameExistsException(author.Name);
        }

        await context.Authors.AddAsync(author, cancellationToken).ConfigureAwait(false);
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return author;
    }

    [Error(typeof(AuthorNotFoundException))]
    [Error(typeof(AuthorWithNameExistsException))]
    public async Task<Author> UpdateAuthorAsync(int id, Author updatedAuthor, [Service] BookContext context, CancellationToken cancellationToken)
    {
        var author = await context.Authors.FindAsync([id], cancellationToken).ConfigureAwait(false);

        if (author is null)
        {
            throw new AuthorNotFoundException(id);
        }

        var authorsWithSameName = await context.Authors.AsNoTracking().Where(a => a.Id != author.Id && a.Name == author.Name).ToListAsync().ConfigureAwait(false);

        if (authorsWithSameName.Any())
        {
            throw new AuthorWithNameExistsException(author.Name);
        }

        author.Name = updatedAuthor.Name;

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return author;
    }

    public async Task<bool> DeleteAuthorAsync(int id, [Service] BookContext context, CancellationToken cancellationToken)
    {
        var author = await context.Authors.FindAsync([id], cancellationToken).ConfigureAwait(false);
        
        if (author is null)
        {
            return false;
        }
        
        context.Authors.Remove(author);
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return true;
    }
}
