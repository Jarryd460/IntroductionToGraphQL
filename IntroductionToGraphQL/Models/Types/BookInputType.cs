namespace IntroductionToGraphQL.Models.Types;

public class BookInputType
    : InputObjectType<Book>
{
    protected override void Configure(
        IInputObjectTypeDescriptor<Book> descriptor)
    {
        descriptor
            .Field(p => p.Id)
            .Ignore(); // Ignore the Id field for input type, as it is not needed when creating a new book

        descriptor
            .Field(p => p.Author)
            .Ignore(); // Ignore the AuthorId field for input type, as it is not needed when creating a new book. We use AuthorId instead
    }
}