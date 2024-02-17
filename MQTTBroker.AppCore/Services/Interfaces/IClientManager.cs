using MQTTBroker.AppCore.Commands.RequestCommands;

namespace MQTTBroker.AppCore.Services.Interfaces
{
    public interface IClientManager
    {
        Task AddConnection(CreateTcpConnectionCommand createTcpConnectionCommand);
        Task EstablishConnection(ConnectCommand connectCommand);
        Task Ping(PingReqCommand pingReqCommand);
    }
}