namespace MQTTBroker.AppCore.Services.Interface;

internal interface ISubscribeOperationsService
{
    void SubscribeTopic(string topicName, TcpConnection subscriber);

    void UnsubscribeTopic(string topicName, TcpConnection subscriber);
}