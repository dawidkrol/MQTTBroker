using MQTTBroker.AppCore.Services;

namespace MQTTBroker.AppCore.Commands;

public class ConnectCommand : ICommand
{
    private readonly byte[] _data;
    public TcpConnection TcpConnection { get; }
    
    private const string ProtocolName = "MQIsdp";
    private const ushort ProtocolVersionNumber = 3;
    private const ushort KeepAliveTimer = 120;

    public ConnectCommand(byte[] data, TcpConnection tcpConnection)
    {
        _data = data;
        TcpConnection = tcpConnection;
    }
}