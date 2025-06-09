using Bogus;
using IntroductionToGraphQL.Infrastructure;
using IntroductionToGraphQL.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddDbContext<BookContext>(options => options.UseInMemoryDatabase("BookDb").UseAsyncSeeding(async (context, _, cancellationToken) =>
{
    var faker = new Faker<Book>()
        // Use seed to ensure consistent data generation across runs
        .UseSeed(100)
        .RuleFor(b => b.Id, f => f.IndexFaker + 1) // Ensure Id starts from 1
        .RuleFor(b => b.Title, f => f.Lorem.Sentence(3))
        .RuleFor(b => b.Author, f => f.Name.FullName())
        .RuleFor(b => b.Price, f => f.Finance.Amount(5, 100));

    var booksToSeed = faker.Generate(5);

    var contains = await context.Set<Book>().ContainsAsync(booksToSeed[0], cancellationToken: cancellationToken);

    if (!contains)
    {
        context.Set<Book>().AddRange(booksToSeed);
        await context.SaveChangesAsync(cancellationToken);
    }
}));

builder.Services.AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddFiltering()
    .AddSorting();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

await using(var serviceScope = app.Services.CreateAsyncScope())
await using (var dbContext = serviceScope.ServiceProvider.GetRequiredService<BookContext>())
{
    // UseAsyncSeeding is called when EnsureCreatedAsync is called.
    // When calling EnsureCreated, then instead of UseAsyncSeeding being called, UseSeeding is called.
    // So we need to ensure we setup the correct seed method to be called
    await dbContext.Database.EnsureCreatedAsync();
}

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapGraphQL();

app.Run();
