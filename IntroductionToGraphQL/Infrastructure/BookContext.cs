using IntroductionToGraphQL.Models;
using Microsoft.EntityFrameworkCore;

namespace IntroductionToGraphQL.Infrastructure;

public sealed class BookContext : DbContext
{
    public DbSet<Book> Books { get; set; } = null!;

    public BookContext(DbContextOptions<BookContext> options)
        : base(options)
    {
        
    }
}
