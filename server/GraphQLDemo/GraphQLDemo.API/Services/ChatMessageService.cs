using GraphQLDemo.API.Dto;
using System.Collections.Concurrent;

namespace GraphQLDemo.API.Services;

//  TODO use Async/Await

// Add interface for chat message service
public interface IChatMessageService
{
    IEnumerable<ChatMessage> GetMessages();

    ChatMessage GetMessage(string id);

    string AddMessage(string sender, string content);
    void ClearMessages();
}


public class ChatMessageService : IChatMessageService
{
    // ConcurrentBag is thread-safe and allows for concurrent access
    private readonly ConcurrentBag<ChatMessage> _messages = new();

    public ChatMessageService()
    {
        // use AddMessage method to add messages
        AddMessage("Alice", "Hello, world!");
        AddMessage("Bob", "Hi, Alice! How are you?");
        AddMessage("Alice", "I'm good, thanks!");
    }

    public IEnumerable<ChatMessage> GetMessages()
    {
        return _messages;
    }

    public ChatMessage GetMessage(string id)
    {
        return _messages.FirstOrDefault(m => m.Id == id);
    }

    public string AddMessage(string sender, string content)
    {
        var message = new ChatMessage(sender, content);
        _messages.Add(message);
        return message.Id;
    }

    public void ClearMessages()
    {
        _messages.Clear();
    }
}

