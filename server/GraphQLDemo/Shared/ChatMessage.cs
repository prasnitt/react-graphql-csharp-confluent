namespace Shared;

public class ChatMessage
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Sender { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.Now;

    public ChatMessage(string sender, string content)
    {
        Sender = sender;
        Content = content;
    }
}

