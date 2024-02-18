using MQTTBroker.AppCore.Commands.RequestCommands;

namespace MQTTBroker.AppCore.Services.Interfaces
{
    public interface IConnectionManager
    {
        Task AddConnection(CreateTcpConnectionCommand createTcpConnectionCommand);
        void RemoveTcpConnection(DisconnectCommand disconnectCommand);
        void RemoveTcpConnection(RemoveDisconnectedClientCommand removeDisconnectedClientCommand);
        Task EstablishConnection(ConnectCommand connectCommand);
        Task Ping(PingReqCommand pingReqCommand);
    }
}