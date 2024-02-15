using MQTTBroker.AppCore.Commands;
using MQTTBroker.AppCore.Services.Interfaces;

namespace MQTTBroker.AppCore.Services.Interface;

public interface IBroker
{
    void Start();

    void AddCommandToQueue(ICommand command);

    Task SendResponce(IResponceCommand responce, ITcpConnection connection);
}