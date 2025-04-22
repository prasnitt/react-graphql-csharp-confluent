using Microsoft.AspNetCore.SignalR;
using Shared;

namespace ChatHub;

public interface IChatClient
{
    Task ReceiveMessage(ChatMessage message);
}
public class ChatHub : Hub<IChatClient>
{
    private readonly ILogger<ChatHub> _logger;

    public ChatHub(ILogger<ChatHub> logger)
    {
        _logger = logger;
        _logger.LogInformation("ChatHub is Active.");
    }

    public override async Task OnConnectedAsync()
    {
        var clientName = Context.GetHttpContext()?.Request.Query["name"];
        var content = $" {clientName} with connection {Context.ConnectionId} has joined";
        _logger.LogInformation(content);

        var chatMessage = new ChatMessage("System", content);
        await Clients.All.ReceiveMessage(chatMessage);
    }

    //[HubMethodName("PublishMessageToOthers")]
    public async Task SendMessage(ChatMessage message)
    {
        await Clients.All.ReceiveMessage(message);
    }
}
