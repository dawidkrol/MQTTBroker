using System.Net.Sockets;

namespace MQTTBroker.AppCore.Commands;

public class CreateTcpConnectionCommand : ICommand
{
    public TcpClient TcpClient { get; }

    public CreateTcpConnectionCommand(TcpClient tcpClient)
    {
        TcpClient = tcpClient;
    }
}