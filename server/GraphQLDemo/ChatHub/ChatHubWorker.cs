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
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }
            await Task.Delay(1000, stoppingToken);

            //await _hubContext.Clients.All.ReceiveMessage($"Worker running at: {DateTimeOffset.Now}");
            var message = _consumer.Consume(stoppingToken);
            await SendMessageToClients(message.Message.Value);
        }
    }

    private async Task SendMessageToClients(ChatMessage message)
    {
        await _hubContext.Clients.All.ReceiveMessage(message);
    }
}