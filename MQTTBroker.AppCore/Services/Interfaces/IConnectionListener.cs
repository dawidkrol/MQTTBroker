using MQTTBroker.AppCore.Services.Interface;

namespace MQTTBroker.AppCore.Services.Interfaces
{
    public interface IConnectionListener
    {
        void Dispose();
        void StartListening(IBroker broker);
        void StopListening();
    }
}