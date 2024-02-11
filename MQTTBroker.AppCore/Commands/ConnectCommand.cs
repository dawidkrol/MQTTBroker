using System.Net.Sockets;

namespace MQTTBroker.AppCore.Commands;

public class ConnectCommand : ICommand
{
    public TcpClient Client { get; }

    public ConnectCommand(TcpClient client)
    {
        Client = client;
    }
}