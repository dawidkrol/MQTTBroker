using MQTTBroker.AppCore.Services;

namespace MQTTBroker.AppCore.Commands.RequestCommands;

public class PublishCommand : ICommand
{
    private readonly byte[] _data;
    public TcpConnection TcpConnection { get; }

    public string TopicName { get; set; }
    public int MessageId { get; set; }
    public byte[] Payload { get; set; }

    public PublishCommand(byte[] data, TcpConnection tcpConnection)
    {
        _data = data;
        TcpConnection = tcpConnection;
        ExtractData();
    }

    private void ExtractData()
    {
        var topicLength = _data[0] << 8 | _data[1];
        TopicName = System.Text.Encoding.UTF8.GetString(_data[2..(topicLength + 2)]);
        MessageId = _data[topicLength + 2] << 8 | _data[topicLength + 4];
        Payload = _data[(topicLength + 5)..];
    }
}