using System.Text;
using MQTTBroker.AppCore.Services.Interfaces;

namespace MQTTBroker.AppCore.Commands.RequestCommands;

public class PublishCommand : ICommand, IResponseCommand
{
    private readonly byte[] _data;
    private readonly byte[] _originalMessage;
    public ITcpConnection TcpConnection { get; }

    public string TopicName { get; set; }
    public int MessageId { get; set; }
    public byte[] Payload { get; set; }

    public PublishCommand(byte[] data, ITcpConnection tcpConnection, byte[] originalMessage)
    {
        _data = data;
        _originalMessage = originalMessage;
        TcpConnection = tcpConnection;
        ExtractData();
    }

    private void ExtractData()
    {
        var topicLength = _data[0] << 8 | _data[1];
        TopicName = Encoding.UTF8.GetString(_data[2..(topicLength + 2)]);
        if (_data.Length > topicLength + 4)
        {
            MessageId = _data[topicLength + 2] << 8 | _data[topicLength + 4];
            Payload = _data[(topicLength + 5)..];
        }
        else
        {
            Payload = _data[(topicLength + 2)..];
        }
    }

    public byte[] ToBuffer()
    {
        Console.WriteLine($"Sending message: \"{Encoding.UTF8.GetString(_originalMessage)}\": {BitConverter.ToString(_originalMessage)}");
        return _originalMessage;
    }
}