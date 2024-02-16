namespace MQTTBroker.AppCore.Commands;

public interface IResponseCommand : ICommand
{
    byte[] ToBuffer();
}
