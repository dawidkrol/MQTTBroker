using MQTTBroker.AppCore.Services;

namespace MQTTBroker.AppCore.Commands;

public class PingReqCommand : ICommand
{
    TcpConnection _tcpConnection;
    
    public PingReqCommand(TcpConnection tcpConnection)
    {
        _tcpConnection = tcpConnection;
    }
}