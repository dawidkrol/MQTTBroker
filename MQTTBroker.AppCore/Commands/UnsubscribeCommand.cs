using MQTTBroker.AppCore.Services;

namespace MQTTBroker.AppCore.Commands;

public class UnsubscribeCommand : ICommand
{
    private readonly byte[] _data;
    public string TopicName { get; set; }
    public string MessageId { get; set; }
    public TcpConnection TcpConnection { get; set; }
    
    public UnsubscribeCommand(byte[] data, TcpConnection tcpConnection)
    {
        _data = data;
        TcpConnection = tcpConnection;
        ExtractData();
    }
    
    private void ExtractData()
    {
        MessageId = System.Text.Encoding.UTF8.GetString(_data[..2]);
        var topicLength = _data[2] << 8 | _data[3];
        TopicName = System.Text.Encoding.UTF8.GetString(_data[4..(topicLength + 4)]);
    }
}