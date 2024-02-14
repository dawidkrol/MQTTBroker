using MQTTBroker.AppCore.Services;

namespace MQTTBroker.AppCore.Commands.RequestCommands;

public class SubscribeCommand : ICommand
{
    private readonly byte[] _data;

    public string MessageId { get; set; }
    public string TopicName { get; set; }
    public TcpConnection TcpConnection { get; set; }


    public SubscribeCommand(byte[] _data, TcpConnection tcpConnection)
    {
        this._data = _data;
        TcpConnection = tcpConnection;
    }

    // the client can subscribe only one topic at a time
    public void ExtractData()
    {
        MessageId = System.Text.Encoding.UTF8.GetString(_data[..2]);
        var topicLength = _data[2] << 8 | _data[3];
        TopicName = System.Text.Encoding.UTF8.GetString(_data[4..(topicLength + 4)]);
    }
}