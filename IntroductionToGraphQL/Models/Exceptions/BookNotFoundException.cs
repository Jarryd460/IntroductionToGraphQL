namespace IntroductionToGraphQL.Models.Exceptions;

internal class BookNotFoundException : Exception
{
    public BookNotFoundException(int id)
        : base($"Book with Id: {id} not found")
    {
    }
}