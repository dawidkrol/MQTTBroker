namespace MQTTBroker.AppCore.Commands;

public interface IResponceCommand : ICommand
{
    byte[] ToBuffer();
}
