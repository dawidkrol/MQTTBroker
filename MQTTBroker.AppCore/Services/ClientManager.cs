using System.Net.Sockets;

namespace MQTTBroker.AppCore.Services;

public class ClientManager
{
    private readonly List<TcpConnection> _connections;
    
    public void AddTcpConnection(TcpClient client)
    {
        var tcpConnection = new TcpConnection(client);
        _connections.Add(tcpConnection);
        tcpConnection.StartAsync();
    }
    
    public void ChangeConnectionStatus(TcpConnection connection, bool status)
    {
        connection.IsConnectionEstablished = status;
    }
}