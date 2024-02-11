using MQTTBroker.AppCore.Services;

namespace MQTTBroker.AppCore.Commands;

public class ConnectCommand : ICommand
{
    private readonly byte[] _data;
    private readonly TcpConnection _tcpConnection;
    
    private const string ProtocolName = "MQIsdp";
    private const ushort ProtocolVersionNumber = 3;
    private const ushort KeepAliveTimer = 120;

    public ConnectCommand(byte[] data, TcpConnection tcpConnection)
    {
        _data = data;
        _tcpConnection = tcpConnection;
    }
}