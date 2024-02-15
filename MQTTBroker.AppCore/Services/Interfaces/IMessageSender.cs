using MQTTBroker.AppCore.Commands.RequestCommands;

namespace MQTTBroker.AppCore.Services.Interface;

public interface IMessageSender
{
    Task PublishMessage(PublishCommand publishCommand);
}