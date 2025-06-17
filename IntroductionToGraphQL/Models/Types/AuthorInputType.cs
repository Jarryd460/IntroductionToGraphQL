namespace IntroductionToGraphQL.Models.Types;

public class AuthorInputType
    : InputObjectType<Author>
{
    protected override void Configure(
        IInputObjectTypeDescriptor<Author> descriptor)
    {
        descriptor
            .Field(p => p.Id)
            .Ignore(); // Ignore the Id field for input type, as it is not needed when creating a new book
    }
}