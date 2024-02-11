namespace MQTTBroker.AppCore.Services.Interface;

public interface IBroker
{
    void NewSubscribtion();
    
    void RemoveSubscription();
    
    void Publish();
    
    void Start();
}