# IntroductionToGraphQL

Learn GraphQL by building a simple book store API with .NET 8 and Entity Framework Core.

## Run the project

Before running the Aspire AppHost project, run the below docker command to start the SQL Server container (it is needed by the IntroductionToGraphQL project):

```bash
docker run -d -e ACCEPT_EULA=Y -e SA_PASSWORD='$trongP@ssword' -e MSSQL_AGENT_ENABLED=true -p 1433:1433 -v mssql_data:/var/opt/mssql mcr.microsoft.com/mssql/server:2019-latest
```

You can then run the IntroductionToGraphQL.AppHost project in Visual Studio or using the .NET CLI:

```bash
dotnet run --project IntroductionToGraphQL.AppHost
```

## Example queries

```graphql
query firstquery {
  books(where: { title: { contains: "a" } }, order: { price: ASC }) {
    title,
    author,
    price
  }
}
```

```graphql
mutation {
    addBook(book: { id: 0, title: "GraphQL for .NET Developers", author: "Jane Doe", price: 29.99 }) {
        id,
        title,
        author,
        price
    }
}
```