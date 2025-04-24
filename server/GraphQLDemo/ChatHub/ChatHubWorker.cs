using Confluent.Kafka;
using Microsoft.AspNetCore.SignalR;
using Shared;

namespace ChatHub;

public class ChatHubWorker : BackgroundService
{
    private readonly ILogger<ChatHubWorker> _logger;
    private readonly IHubContext<ChatHub, IChatClient> _hubContext;
    private readonly IConsumer<String, ChatMessage> _consumer;
    public ChatHubWorker(ILogger<ChatHubWorker> logger, IHubContext<ChatHub, IChatClient> hubContext, IConsumer<String, ChatMessage> consumer)
    {
        _hubContext = hubContext;
        _logger = logger;
        _consumer = consumer;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _consumer.Subscribe(Constants.ChatHubTopic);
        while (!stoppingToken.IsCancellationRequested)
        {
            var message = _consumer.Consume(TimeSpan.FromMilliseconds(100));

            if (message != null)
            {
                _logger.LogInformation($"Received message: {message.Message.Value.ToLogMessage()}");
                await SendMessageToClients(message.Message.Value);
            }
            else
            {
                //_logger.LogWarning("No message received");
                // Avoid tight loop if no message is available
                await Task.Delay(10, stoppingToken);
            }
        }
    }

    private async Task SendMessageToClients(ChatMessage message)
    {
        await _hubContext.Clients.All.ReceiveMessage(message);
    }
}