namespace IntroductionToGraphQL.Models.Subscriptions;

[SubscriptionType]
public sealed class BookSubscription
{
    [Subscribe]
    public Book BookAdded([EventMessage] Book book)
    {
        return book;
    }

    [Subscribe]
    [Topic($"{{{nameof(authorId)}}}")]
    public Book BookUpdated(int authorId, [EventMessage] Book book)
    {
        return book;
    }
}
