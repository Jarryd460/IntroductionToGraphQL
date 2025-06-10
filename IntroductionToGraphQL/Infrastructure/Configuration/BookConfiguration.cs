using IntroductionToGraphQL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IntroductionToGraphQL.Infrastructure.Configuration;

internal class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.Property(book => book.Title)
            .HasMaxLength(100);

        builder.Property(book => book.Price)
            .HasPrecision(5, 2);
    }
}
