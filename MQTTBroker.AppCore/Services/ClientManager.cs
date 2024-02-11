using System.Net.Sockets;

namespace MQTTBroker.AppCore.Services;

public class ClientManager
{
    private readonly List<TcpConnection> _connections;
    
    public void AddTcpConnection(TcpClient client)
    {
        _connections.Add(new TcpConnection(client));
    }
}