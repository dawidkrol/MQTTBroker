using System.Net.Sockets;
using System.Text.RegularExpressions;
using MQTTBroker.AppCore.Commands.RequestCommands;
using MQTTBroker.AppCore.Commands.ResponseCommands;
using MQTTBroker.AppCore.Enums;
using MQTTBroker.AppCore.Models;
using MQTTBroker.AppCore.Services.Interfaces;

namespace MQTTBroker.AppCore.Services;

public class ConnectionManager : IConnectionManager
{
    private readonly List<ITcpConnection> _connections = new();
    private readonly IBroker _broker;
    private readonly List<User> _users = new()
    {
        new User {Username = "filip", Password = "password"},
        new User {Username = "gieracki", Password = "password2"}
    };

    public ConnectionManager(IBroker broker)
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
        if (connectCommand.Version != 3 && connectCommand.Version != 4)
        {
            await _broker.SendResponse(new ConnAck(ConnackReturnCode.UnacceptableProtocolVersion), connectCommand.TcpConnection);
            _connections.Remove(connectCommand.TcpConnection);
        }
        if (_connections.Any(connection => connection.ClientId == connectCommand.ClientId))
        {
            await _broker.SendResponse(new ConnAck(ConnackReturnCode.IdentifierRejected), connectCommand.TcpConnection);
            _connections.Remove(connectCommand.TcpConnection);
        }
        if (!IsValidCredential(connectCommand.Username) || !IsValidCredential(connectCommand.Password))
        {
            await _broker.SendResponse(new ConnAck(ConnackReturnCode.BadUsernameOrPassword), connectCommand.TcpConnection);
            _connections.Remove(connectCommand.TcpConnection);
        }
        if (_users.Find(user => user.Username == connectCommand.Username && user.Password == connectCommand.Password) == null)
        {
            await _broker.SendResponse(new ConnAck(ConnackReturnCode.NotAuthorized), connectCommand.TcpConnection);
            _connections.Remove(connectCommand.TcpConnection);
        }
        
        ChangeConnectionStatus(connectCommand.TcpConnection, true);
        connectCommand.TcpConnection.ClientId = connectCommand.ClientId;
        await _broker.SendResponse(new ConnAck(ConnackReturnCode.ConnectionAccepted), connectCommand.TcpConnection);
    }
    
    private bool IsValidCredential(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return false;
        }
        Regex regex = new Regex(@"^[\p{IsBasicLatin}]+$");
        return regex.IsMatch(input);
    }

    public async Task AddConnection(CreateTcpConnectionCommand createTcpConnectionCommand)
    {
        await AddTcpConnection(createTcpConnectionCommand.TcpClient);
    }
    
    public void RemoveTcpConnection(DisconnectCommand disconnectCommand)
    {
        disconnectCommand.TcpConnection.Close();
        _connections.Remove(disconnectCommand.TcpConnection);
    }
    
    public void RemoveTcpConnection(RemoveDisconnectedClientCommand removeDisconnectedClientCommand)
    {
        removeDisconnectedClientCommand.TcpConnection.Close();
        _connections.Remove(removeDisconnectedClientCommand.TcpConnection);
    }
    
    public async Task Ping(PingReqCommand pingReqCommand)
    {
        await _broker.SendResponse(new PingResp(), pingReqCommand.TcpConnection);
    }
}