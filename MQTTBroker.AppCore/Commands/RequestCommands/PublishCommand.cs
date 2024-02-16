using System.Text;
using MQTTBroker.AppCore.Services.Interfaces;

namespace MQTTBroker.AppCore.Commands.RequestCommands;

public class PublishCommand : ICommand
{
    private readonly byte[] _data;
    public ITcpConnection TcpConnection { get; }

    public string TopicName { get; set; }
    public int MessageId { get; set; }
    public byte[] Payload { get; set; }

    public PublishCommand(byte[] data, ITcpConnection tcpConnection)
    {
        _data = data;
        TcpConnection = tcpConnection;
        ExtractData();
    }

    private void ExtractData()
    {
        var topicLength = _data[0] << 8 | _data[1];
        TopicName = Encoding.UTF8.GetString(_data[2..(topicLength + 2)]);
        MessageId = _data[topicLength + 2] << 8 | _data[topicLength + 4];
        Payload = _data[(topicLength + 5)..];
    }
}