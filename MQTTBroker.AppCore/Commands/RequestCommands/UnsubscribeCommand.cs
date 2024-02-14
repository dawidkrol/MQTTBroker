using MQTTBroker.AppCore.Services;

namespace MQTTBroker.AppCore.Commands.RequestCommands;

public class UnsubscribeCommand : ICommand
{
    private readonly byte[] _data;
    public string TopicName { get; set; }
    public int MessageId { get; set; }
    public TcpConnection TcpConnection { get; set; }

    public UnsubscribeCommand(byte[] data, TcpConnection tcpConnection)
    {
        _data = data;
        TcpConnection = tcpConnection;
        ExtractData();
    }

    private void ExtractData()
    {
        MessageId = (_data[0] << 8) | _data[1];
        var topicLength = _data[2] << 8 | _data[3];
        TopicName = System.Text.Encoding.UTF8.GetString(_data[4..(topicLength + 4)]);
    }
}