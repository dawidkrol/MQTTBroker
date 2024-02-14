namespace MQTTBroker.AppCore.Commands.ResponseCommands;

public class UnsubAck
{
    public ushort MessageId { get; private set; }

    public UnsubAck(ushort messageId)
    {
        MessageId = messageId;
    }

    public byte[] ToBuffer()
    {
        var builder = CommandBuilder
                        .TotalLenghOfCommand(4)
                            .AddCommandType(Enums.MessageType.UnsubAck)
                            .AddData(2)
                            .AddMessageId(MessageId);

        return builder.Build();
    }
}
