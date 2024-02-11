using MQTTBroker.AppCore.Services;

namespace MQTTBroker.AppCore.Commands;

public interface ICommandFactory
{
    public ICommand CreateCommand(byte[] data, TcpConnection tcpConnection);
}