namespace IntroductionToGraphQL.Models.Exceptions;

internal sealed class AuthorWithNameExistsException : Exception
{
    public AuthorWithNameExistsException(string name)
        : base($"An author with the name '{name}' already exists.")
    {
    }
}