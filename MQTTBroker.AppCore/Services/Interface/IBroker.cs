using MQTTBroker.AppCore.Commands;

namespace MQTTBroker.AppCore.Services.Interface;

public interface IBroker
{
    void Start();
    
    void ExecuteCommand(ICommand command);
}