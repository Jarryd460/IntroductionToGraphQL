# IntroductionToGraphQL

Learn GraphQL by building a simple book store API with .NET 8 and Entity Framework Core.

## Run the project

Before running the Aspire AppHost project, run the below docker command to start the SQL Server container (it is needed by the IntroductionToGraphQL project):

```bash
docker run -d -e ACCEPT_EULA=Y -e SA_PASSWORD='$trongP@ssword' -e MSSQL_AGENT_ENABLED=true -p 1433:1433 -v mssql_data:/var/opt/mssql mcr.microsoft.com/mssql/server:2019-latest
```

You need to run the Redis Stack container to enable the GraphQL subscriptions feature. Use the following command to start the Redis Stack container (ensure you are in the Git root directory of the project):
Ensure that the redis.conf file is UTF-8 encoded and does not contain a BOM (Byte Order Mark) and Line Endings are set to LF (Unix style).

```bash
docker run -d --name redis-stack -p 6379:6379 -p 8001:8001 -v redis_data:/data -v $pwd/redis.conf:/redis-stack.conf redis/redis-stack:latest
```

You can use environment variables instead of a redis.conf file

```bash
docker run -d --name redis-stack -e REDIS_ARGS="--appendonly yes --save 60 1 --dir /data" -p 6379:6379 -p 8001:8001 -v redis_data:/data redis/redis-stack:latest
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
    author {
		id,
		name
	}
    price
  }
}
```

```graphql
query singleBook($id: ID!) {
  book(id: $id) {
		id,
    title,
    price,
	authorId,
	author {
		id,
		name
	}
  }
}

//variables
{
  "id": 1
}
```

```graphql
mutation {
	addAuthor(input: { author: { name: "John" }}) {
		author {
			id,
			name
		}
	}
}
```

More queries are available in the insomnia json file in the root directory. You will need to download insomnia and import the requests into insomnia to run the requests