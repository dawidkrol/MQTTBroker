using MQTTBroker.AppCore.Services;

namespace MQTTBroker.AppCore.Commands.RequestCommands;

public class RemoveDisconnectedClientCommand : ICommand
{
    public TcpConnection TcpConnection { get; set; }
    
    public RemoveDisconnectedClientCommand(TcpConnection tcpConnection)
    {
        TcpConnection = tcpConnection;
    }
}