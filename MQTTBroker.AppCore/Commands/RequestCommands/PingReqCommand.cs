using MQTTBroker.AppCore.Services;

namespace MQTTBroker.AppCore.Commands.RequestCommands;

public class PingReqCommand : ICommand
{
    TcpConnection _tcpConnection;

    public PingReqCommand(TcpConnection tcpConnection)
    {
        _tcpConnection = tcpConnection;
    }
}