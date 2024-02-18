namespace MQTTBroker.AppCore.Services.Interfaces
{
    public interface IConnectionListener
    {
        void Dispose();
        void StartListening();
        void StopListening();
    }
}