namespace MQTTBroker.AppCore.Models
{
    public interface IBroker
    {
        void NewSubscribtion();
        void Publish();
        void RemoveSubscription();
        void Start();
    }
}