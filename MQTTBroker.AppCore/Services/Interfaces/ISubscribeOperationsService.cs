using MQTTBroker.AppCore.Commands.RequestCommands;

namespace MQTTBroker.AppCore.Services.Interface;

public interface ISubscribeOperationsService
{
    Task SubscribeTopic(SubscribeCommand subscribeCommand);

    Task UnsubscribeTopic(UnsubscribeCommand unsubscribeCommand);
}