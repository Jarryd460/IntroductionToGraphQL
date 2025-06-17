using IntroductionToGraphQL.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddDbContext<BookContext>();

builder.Services.AddGraphQLServer()
    // Manually add types if you don't want to use the source generated extension method
    // Only a single QueryType can be registered. It is the entrypoint
    //.AddQueryType<BookQuery>()
    //.AddType<BookType>()
    // Only a single MutationType can be registered. It is the entrypoint
    //.AddMutationType<Mutation>()
    // Source generated extension method from HotChocolate.Types.Analyzers package
    .AddIntroductionToGraphQLTypes()
    .AddFiltering()
    .AddSorting()
    .AddProjections()
    // Generates AddAuthorInput, AddAuthorPayload, UpdateAuthorInput, UpdateAuthorPayload, DeleteAuthorInput, DeleteAuthorPayload which is the recommeded pattern by HotChocolate for GraphQL
    .AddMutationConventions()
    // Wraps all mutations into a single transaction and if all succeeds it gets committed otherwise if any mutation fails then all are rolledback. SaveChangesAsync calls are wrapped internally
    // so that changes are not committed immediately when called. This is done internally by AddDefaultTransactionScopeHandler.
    .AddDefaultTransactionScopeHandler();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

await using (var serviceScope = app.Services.CreateAsyncScope())
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
