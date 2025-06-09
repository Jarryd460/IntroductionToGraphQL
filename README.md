# IntroductionToGraphQL

Learn GraphQL by building a simple book store API with .NET 8 and Entity Framework Core.

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