using MQTTBroker.AppCore.Services.Interfaces;

namespace MQTTBroker.AppCore.Commands.RequestCommands;

public class PingReqCommand : ICommand
{
    ITcpConnection _tcpConnection;

    public PingReqCommand(ITcpConnection tcpConnection)
    {
        _tcpConnection = tcpConnection;
    }
}