using IntroductionToGraphQL.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace IntroductionToGraphQL.Models.Types;

internal sealed class BookType : ObjectType<Book>
{
    protected override void Configure(IObjectTypeDescriptor<Book> descriptor)
    {
        descriptor.Field(b => b.Author)
             .ResolveWith<BookResolvers>(r => r.GetAuthorAsync(default!, default!, default!));

        descriptor
            .Field(p => p.Id)
            .Type<NonNullType<IdType>>();
    }

    private sealed class BookResolvers
    {
        public async Task<Author?> GetAuthorAsync([Parent] Book book, BookContext context, CancellationToken cancellation)
        {
            return await context.Authors.AsNoTracking().SingleOrDefaultAsync(author => author.Id == book.AuthorId, cancellation).ConfigureAwait(false);
        }
    }
}
