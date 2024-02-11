using MQTTBroker.AppCore.Services;

namespace MQTTBroker.AppCore.Commands;

public class DisconnectCommand : ICommand
{
    public TcpConnection TcpConnection { get; set; }
    
    public DisconnectCommand(TcpConnection tcpConnection)
    {
        TcpConnection = tcpConnection;
    }
}