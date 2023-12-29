namespace MQTTBroker.AppCore.Models
{
    internal interface ISubscribeOperationsService
    {
        void SubscribeTopic(int topicId, SubscriberModel subscriber);

        void UnsubscribeTopic(int topicId, Guid subscriberId);
    }
}