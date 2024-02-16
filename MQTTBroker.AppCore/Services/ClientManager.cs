using System.Net.Sockets;
using MQTTBroker.AppCore.Commands.RequestCommands;
using MQTTBroker.AppCore.Commands.ResponseCommands;
using MQTTBroker.AppCore.Enums;
using MQTTBroker.AppCore.Services.Interfaces;

namespace MQTTBroker.AppCore.Services;

public class ClientManager : IClientManager
{
    private readonly List<ITcpConnection> _connections = new();
    private readonly IBroker _broker;

    public ClientManager(IBroker broker)
    {
        _broker = broker;
    }

    private async Task AddTcpConnection(TcpClient client)
    {
        var tcpConnection = new TcpConnection(client, _broker);
        _connections.Add(tcpConnection);
        await tcpConnection.StartAsync();
    }

    private void ChangeConnectionStatus(ITcpConnection connection, bool status)
    {
        connection.IsConnectionEstablished = status;
    }

    public async Task EstablishConnection(ConnectCommand connectCommand)
    {
        ChangeConnectionStatus(connectCommand.TcpConnection, true);
        await _broker.SendResponse(new ConnAck(ConnackReturnCode.ConnectionAccepted), connectCommand.TcpConnection);
    }

    public async Task AddConnection(CreateTcpConnectionCommand createTcpConnectionCommand)
    {
        await AddTcpConnection(createTcpConnectionCommand.TcpClient);
    }
}