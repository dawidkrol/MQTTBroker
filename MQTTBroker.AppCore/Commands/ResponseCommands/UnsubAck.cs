using MQTTBroker.AppCore.Commands.Builder;

namespace MQTTBroker.AppCore.Commands.ResponseCommands;

public class UnsubAck : IResponceCommand
{
    public int MessageId { get; private set; }

    public UnsubAck(int messageId)
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
