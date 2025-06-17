using IntroductionToGraphQL.Infrastructure;

namespace IntroductionToGraphQL.Models;

[MutationType]
public sealed class AuthorMutation
{
    public async Task<Author> AddAuthorAsync(Author author, [Service] BookContext context, CancellationToken cancellationToken)
    {
        await context.Authors.AddAsync(author, cancellationToken).ConfigureAwait(false);
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return author;
    }

    public async Task<Author> UpdateAuthorAsync(int id, Author updatedAuthor, [Service] BookContext context, CancellationToken cancellationToken)
    {
        var author = await context.Authors.FindAsync([id], cancellationToken).ConfigureAwait(false);

        if (author is null)
        {
            throw new Exception($"Author with Id: {id} not found"); ;
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
