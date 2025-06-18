namespace IntroductionToGraphQL.Models.Exceptions;

internal sealed class BookWithTitleExistsException : Exception
{
    public BookWithTitleExistsException(string title)
        : base($"A book with the title '{title}' already exists.")
    {
    }
}