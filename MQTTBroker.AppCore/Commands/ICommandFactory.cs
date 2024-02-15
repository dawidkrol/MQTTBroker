using MQTTBroker.AppCore.Services.Interfaces;

namespace MQTTBroker.AppCore.Commands;

public interface ICommandFactory
{
    public ICommand CreateCommand(byte[] data, ITcpConnection tcpConnection);
}