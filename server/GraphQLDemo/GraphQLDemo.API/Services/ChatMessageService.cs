using Confluent.Kafka;
using Shared;
using System.Collections.Concurrent;

namespace GraphQLDemo.API.Services;

//  TODO use Async/Await

// Add interface for chat message service
public interface IChatMessageService
{
    IEnumerable<ChatMessage> GetMessages();

    ChatMessage GetMessage(string id);

    Task<string> AddMessageAsync(string sender, string content);
    void ClearMessages();
}


public class ChatMessageService : IChatMessageService
{
    // ConcurrentBag is thread-safe and allows for concurrent access
    // TODO: remove this and use Kafka
    private readonly ConcurrentBag<ChatMessage> _messages = new();

    private readonly ILogger<ChatMessageService> _logger;
    private readonly IProducer<String, ChatMessage> _producer;

    public ChatMessageService(IProducer<String, ChatMessage> producer, ILogger<ChatMessageService> logger)
    {
        _logger = logger;
        _producer = producer;

        logger.LogInformation("ChatMessageService is Active.");
        // use AddMessage method to add messages
        //AddMessage("Alice", "Hello, world!");
        //AddMessage("Bob", "Hi, Alice! How are you?");
        //AddMessage("Alice", "I'm good, thanks!");
    }

    public IEnumerable<ChatMessage> GetMessages()
    {
        return _messages;
    }

    public ChatMessage GetMessage(string id)
    {
        return _messages.FirstOrDefault(m => m.Id == id);
    }

    public async Task<string> AddMessageAsync(string sender, string content)
    {
        var message = new ChatMessage(sender, content);
        _messages.Add(message);

        // Send message to Kafka
        await SendMessageToKafkaAsync(message);

        return message.Id;
    }

    public void ClearMessages()
    {
        _messages.Clear();
    }

    private async Task SendMessageToKafkaAsync(ChatMessage chatMessage)
    {
        try
        {
            var message = new Message<string, ChatMessage>
            {
                Key = chatMessage.Id,
                Value = chatMessage
            };

            var result = await _producer.ProduceAsync("chat-messages", message);
            _producer.Flush();

            _logger.LogInformation($"Message sent to Kafka: {result.Value}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending message to Kafka");
        }
    }
}

