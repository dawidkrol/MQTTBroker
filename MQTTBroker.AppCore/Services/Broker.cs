namespace MQTTBroker.AppCore.Models
{
    public class Broker(int port, string host) : IBroker
    {
        private readonly ConnectionReceiver _connectionReceiver = new(port, host);

        public void Start()
        {
            _connectionReceiver.StartListening(this, AddNewClient, RemoveClient);
        }

        private void RemoveClient(SubscriberModel client)
        {
        }

        private void AddNewClient(SubscriberModel client)
        {
        }

        public void NewSubscribtion() { }

        public void RemoveSubscription() { }

        public void Publish() { }
    }
}
