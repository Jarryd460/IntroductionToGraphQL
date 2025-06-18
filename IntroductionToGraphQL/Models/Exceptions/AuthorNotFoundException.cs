namespace IntroductionToGraphQL.Models.Exceptions;

internal class AuthorNotFoundException : Exception
{
    public AuthorNotFoundException(int id)
        : base($"Author with Id: {id} not found")
    {
    }
}