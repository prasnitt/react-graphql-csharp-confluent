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
    private readonly ClientConnectionManager _clientManager;

    public ChatHub(ILogger<ChatHub> logger, ClientConnectionManager clientManager)
    {
        _logger = logger;
        _clientManager = clientManager;
        _logger.LogInformation("ChatHub is Active.");
    }

    public override async Task OnConnectedAsync()
    {
        var clientName = Context.GetHttpContext()?.Request.Query["name"];
        clientName ??= "Unknown"; // Fallback if name is not provided
        var content = $" 👋 '{clientName}' has just joined. 👋 ";
        _logger.LogInformation(content);


        _clientManager.AddClient(Context.ConnectionId, clientName);
        var chatMessage = new ChatMessage("System", content);
        await Clients.All.ReceiveMessage(chatMessage);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {

        if (_clientManager.TryGetClient(Context.ConnectionId, out string clientName))
        {
            var content = $" '{clientName}' has just left. 😪 😪";
            _logger.LogInformation(content);

            var chatMessage = new ChatMessage("System", content);
            await Clients.Others.ReceiveMessage(chatMessage);
            _clientManager.RemoveClient(Context.ConnectionId);
        }
        else
        {
            _logger.LogWarning("Client name not found for connection ID: {ConnectionId}", Context.ConnectionId);
        }
    }

    //[HubMethodName("PublishMessageToOthers")]
    public async Task SendMessage(ChatMessage message)
    {
        await Clients.All.ReceiveMessage(message);
    }
}
