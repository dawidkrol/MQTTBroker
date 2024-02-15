using MQTTBroker.AppCore.Services.Interfaces;

namespace MQTTBroker.AppCore.Commands.RequestCommands;

public class ConnectCommand : ICommand
{
    private readonly byte[] _data;
    public ITcpConnection TcpConnection { get; }

    private const string ProtocolName = "MQIsdp";
    private const ushort ProtocolVersionNumber = 3;
    private const ushort KeepAliveTimer = 120;

    public ConnectCommand(byte[] data, ITcpConnection tcpConnection)
    {
        _data = data;
        TcpConnection = tcpConnection;
    }
}