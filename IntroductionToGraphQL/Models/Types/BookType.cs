using IntroductionToGraphQL.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace IntroductionToGraphQL.Models.Types;

internal sealed class BookType : ObjectType<Book>
{
    protected override void Configure(IObjectTypeDescriptor<Book> descriptor)
    {
        //descriptor.Field(b => b.Author)
        //    .Type<Author>()      // your Author GraphQL type. I don't have an Author GraphQL type (class) like BookType as I used attributes to define the Author type. I don't think I need it as it's implicity retrieved based on the property type
        //    .UseProjection();        // ← pulls in the Author FK and does a JOIN

        //descriptor.Field(b => b.Author)
        //    .Type<Author>()      // your Author GraphQL type. I don't have an Author GraphQL type (class) like BookType as I used attributes to define the Author type. I don't think I need it as it's implicity retrieved based on the property type
        //    .ResolveWith<BookResolvers>(r => r.GetAuthorAsync(default!, default!, default!));

        descriptor
            .Field(p => p.Id)
            .Type<NonNullType<IdType>>();

        //HotChocolate has support for other Scalar Types that ensures better type safety
        descriptor
            .Field(b => b.EmailAddress)
            .Type<EmailAddressType>();
    }

    private sealed class BookResolvers
    {
        [UseProjection]
        public async Task<Author?> GetAuthorAsync([Parent] Book book, BookContext context, CancellationToken cancellation)
        {
            return await context.Authors.AsNoTracking().SingleOrDefaultAsync(author => author.Id == book.AuthorId, cancellation).ConfigureAwait(false);
        }
    }
}
