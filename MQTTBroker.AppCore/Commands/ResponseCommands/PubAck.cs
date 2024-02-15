using MQTTBroker.AppCore.Commands.Builder;

namespace MQTTBroker.AppCore.Commands.ResponseCommands;

// QoS = 1
public class PubAck : IResponceCommand
{
    public int MessageId { get; private set; }

    public PubAck(int messageId)
    {
        MessageId = messageId;
    }

    public byte[] ToBuffer()
    {
        var builder = CommandBuilder
                            .TotalLenghOfCommand(4)
                                .AddCommandType(Enums.MessageType.PubAck)
                                .AddData(2)
                                .AddMessageId(MessageId);

        return builder.Build();
    }
}
