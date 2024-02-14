using MQTTBroker.AppCore.Services.Interface;
using System.Net.Sockets;

namespace MQTTBroker.AppCore.Services;

public class ClientManager
{
    private readonly List<TcpConnection> _connections;
    
    public async Task AddTcpConnection(TcpClient client, IBroker broker)
    {
        var tcpConnection = new TcpConnection(client, broker);
        _connections.Add(tcpConnection);
        await tcpConnection.StartAsync();
    }
    
    public void ChangeConnectionStatus(TcpConnection connection, bool status)
    {
        connection.IsConnectionEstablished = status;
    }
}