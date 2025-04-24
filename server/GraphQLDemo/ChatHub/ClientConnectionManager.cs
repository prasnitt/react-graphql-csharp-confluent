namespace ChatHub;

public class ClientConnectionManager
{
    private readonly Dictionary<string, string> _clients = new Dictionary<string, string>();
    private readonly object _lock = new object();

    public void AddClient(string connectionId, string clientName)
    {
        lock (_lock)
        {
            _clients[connectionId] = clientName;
        }

    }

    public void RemoveClient(string connectionId)
    {
        if (_clients.TryGetValue(connectionId, out string clientName))
        {
            _clients.Remove(connectionId);
        }
    }

    public bool TryGetClient(string connectionId, out string clientName)
    {
        lock (_lock)
        {
            return _clients.TryGetValue(connectionId, out clientName);
        }
    }


}
