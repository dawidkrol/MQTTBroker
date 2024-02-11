namespace MQTTBroker.AppCore.Services.Interface;

internal interface IMessageSender
{
    public void SendMessageToSubscribers(int topicId, string message);
}