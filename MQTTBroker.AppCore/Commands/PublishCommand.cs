using MQTTBroker.AppCore.Services;

namespace MQTTBroker.AppCore.Commands;

public class PublishCommand : ICommand
{
    private readonly byte[] _data;
    private readonly TcpConnection _tcpConnection;
    
    public string TopicName { get; set; }
    public string MessageId { get; set; }
    public string Payload { get; set; }
    
    public PublishCommand(byte[] data, TcpConnection tcpConnection)
    {
        _data = data;
        _tcpConnection = tcpConnection;
        ExtractData();
    }
    
    private void ExtractData()
    {
        var topicLength = _data[0] << 8 | _data[1];
        TopicName = System.Text.Encoding.UTF8.GetString(_data[2..(topicLength + 2)]);
        var messageIdLength = _data[topicLength + 2] << 8 | _data[topicLength + 3];
        MessageId = System.Text.Encoding.UTF8.GetString(_data[(topicLength + 4)..(topicLength + messageIdLength + 4)]);
        Payload = System.Text.Encoding.UTF8.GetString(_data[(topicLength + messageIdLength + 4)..]);
    }
}