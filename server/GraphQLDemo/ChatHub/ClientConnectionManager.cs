using System.Collections.Concurrent;

namespace ChatHub;

public class ClientConnectionManager
{
    private readonly ConcurrentDictionary<string, string> _clients = new();

    public void AddClient(string connectionId, string clientName)
    {

        _clients[connectionId] = clientName;
    }

    public void RemoveClient(string connectionId)
    {
        _clients.TryRemove(connectionId, out string _);
    }

    public bool TryGetClient(string connectionId, out string clientName)
    {
        return _clients.TryGetValue(connectionId, out clientName);
    }
}
