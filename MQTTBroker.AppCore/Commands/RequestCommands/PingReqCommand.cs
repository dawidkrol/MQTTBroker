using MQTTBroker.AppCore.Services.Interfaces;

namespace MQTTBroker.AppCore.Commands.RequestCommands;

public class PingReqCommand : ICommand
{
    public ITcpConnection TcpConnection;

    public PingReqCommand(ITcpConnection tcpConnection)
    {
        TcpConnection = tcpConnection;
    }
}