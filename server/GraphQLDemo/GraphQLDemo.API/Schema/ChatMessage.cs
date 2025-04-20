namespace GraphQLDemo.API.Schema;

public class ChatMessage
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Sender { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public ChatMessage(string sender, string content)
    {
        Sender = sender;
        Content = content;
    }
}

