using Bogus;
using IntroductionToGraphQL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;

namespace IntroductionToGraphQL.Infrastructure;

public sealed class BookContext : DbContext
{
    private readonly IConfiguration _configuration;
    public DbSet<Book> Books { get; set; } = null!;
    public DbSet<Author> Authors { get; set; } = null!;

    public BookContext(DbContextOptions<BookContext> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        var connectionString = _configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Database connection string is not set.");
        options.UseSqlServer(connectionString)
            // One should use migrations to seed data instead for large datasets. Migrations should be it's own project
            .UseAsyncSeeding(async (context, _, cancellationToken) =>
            {
                var fakeAuthors = new Faker<Author>()
                    // Use seed to ensure consistent data generation across runs
                    .UseSeed(100)
                    .RuleFor(a => a.Name, f => f.Person.FullName);

                var authorsToSeed = fakeAuthors.Generate(5);

                var emptyAuthorsTable = context.Set<Author>().IsNullOrEmpty();

                if (emptyAuthorsTable)
                {
                    var fakeBooks = new Faker<Book>()
                    // Use seed to ensure consistent data generation across runs
                    .UseSeed(100)
                    .RuleFor(b => b.Title, f => f.Lorem.Sentence(3))
                    .RuleFor(b => b.Author, f => f.PickRandom(authorsToSeed)) // Randomly pick an author from the seeded authors
                    .RuleFor(b => b.Price, f => f.Finance.Amount(5, 100));

                    var booksToSeed = fakeBooks.Generate(20);

                    context.Set<Book>().AddRange(booksToSeed);
                    await context.SaveChangesAsync(cancellationToken);
                }
            });
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }
}
