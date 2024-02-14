using System.Net.Sockets;

namespace MQTTBroker.AppCore.Commands.RequestCommands;

public class CreateTcpConnectionCommand : ICommand
{
    public TcpClient TcpClient { get; }

    public CreateTcpConnectionCommand(TcpClient tcpClient)
    {
        TcpClient = tcpClient;
    }
}