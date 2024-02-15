using MQTTBroker.AppCore.Services.Interfaces;

namespace MQTTBroker.AppCore.Commands.RequestCommands;

public class DisconnectCommand : ICommand
{
    public ITcpConnection TcpConnection { get; set; }

    public DisconnectCommand(ITcpConnection tcpConnection)
    {
        TcpConnection = tcpConnection;
    }
}