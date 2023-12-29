namespace MQTTBroker.AppCore.Models
{
    internal interface IMessageSender
    {
        public void SendmMssageToSubscribers(int topicId, string message);
    }
}