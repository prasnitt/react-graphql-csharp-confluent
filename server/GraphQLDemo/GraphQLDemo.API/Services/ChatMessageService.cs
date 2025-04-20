using GraphQLDemo.API.Dto;

namespace GraphQLDemo.API.Services;

// Add interface for chat message service
public interface IChatMessageService
{
    IEnumerable<ChatMessage> GetMessages();

    ChatMessage GetMessage(string id);

    void AddMessage(ChatMessage message);
    void ClearMessages();
}


public class ChatMessageService : IChatMessageService
{
    private readonly List<ChatMessage> _messages = new();

    public ChatMessageService()
    {
        // Seed some initial messages
        _messages.Add(new ChatMessage("Alice", "Hello, world!"));
        _messages.Add(new ChatMessage("Bob", "Hi, Alice! How are you?"));
        _messages.Add(new ChatMessage("Alice", "I'm good, thanks!"));
    }

    public IEnumerable<ChatMessage> GetMessages()
    {
        return _messages;
    }

    public ChatMessage GetMessage(string id)
    {
        return _messages.FirstOrDefault(m => m.Id == id);
    }

    public void AddMessage(ChatMessage message)
    {
        _messages.Add(message);
    }

    public void ClearMessages()
    {
        _messages.Clear();
    }
}

