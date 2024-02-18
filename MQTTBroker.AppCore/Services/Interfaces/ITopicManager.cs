using MQTTBroker.AppCore.Commands.RequestCommands;
using MQTTBroker.AppCore.Models;
using MQTTBroker.AppCore.Services.Interface;

namespace MQTTBroker.AppCore.Services.Interfaces
{
    public interface ITopicManager : ICrudTopicService, ISubscribeOperationsService, IMessageSender
    {
        Topic? GetTopic(string topicName);
        Task PublishMessage(PublishCommand publishCommand);
        void RemoveTcpConnection(DisconnectCommand disconnectCommand);
        void RemoveTcpConnection(RemoveDisconnectedClientCommand disconnectCommand);
        Task SubscribeTopic(SubscribeCommand subscribeCommand);
        Task UnsubscribeTopic(UnsubscribeCommand unsubscribeCommand);
    }
}