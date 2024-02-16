using MQTTBroker.AppCore.Commands;

namespace MQTTBroker.AppCore.Services.Interfaces;

public interface IBroker
{
    void Start();
    
    void Stop();

    void AddCommandToQueue(ICommand command);

    Task SendResponse(IResponseCommand response, ITcpConnection connection);
}