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
        _logger.LogInformation($" {clientName} with connection {Context.ConnectionId} has joined");
        await Clients.All.ReceiveMessage($" {clientName} with connection {Context.ConnectionId} has joined");
    }

    //[HubMethodName("PublishMessageToOthers")]
    public async Task SendMessage(string message)
    {
        await Clients.All.ReceiveMessage(message);
    }
}
