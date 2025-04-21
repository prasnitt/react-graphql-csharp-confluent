using Microsoft.AspNetCore.SignalR;

namespace ChatHub;

public interface IChatClient
{
    Task ReceiveMessage(string message);
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
        _logger.LogInformation($"Client '{Context.ConnectionId}' Connected. ");
        await Clients.Others.ReceiveMessage($" {clientName} with connection {Context.ConnectionId} has joined");
    }

    //[HubMethodName("PublishMessageToOthers")]
    public async Task SendMessage(string message)
    {
        await Clients.Others.ReceiveMessage(message);
    }
}
