using MQTTBroker.AppCore.Models;

namespace MQTTBroker.AppCore.Services.Interface;

internal interface ISubscribeOperationsService
{
    void SubscribeTopic(int topicId, SubscriberModel subscriber);

    void UnsubscribeTopic(int topicId, Guid subscriberId);
}