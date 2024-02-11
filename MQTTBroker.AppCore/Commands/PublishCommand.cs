namespace MQTTBroker.AppCore.Commands;

public class PublishCommand : ICommand
{
    public string TopicName { get; set; }
    public string Message { get; set; }
}