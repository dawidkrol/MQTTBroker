using MQTTBroker.AppCore.Commands.Builder;
using MQTTBroker.AppCore.Enums;

namespace MQTTBroker.AppCore.Commands.ResponseCommands;

public class UnsubAck : IResponseCommand
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
                            .AddCommandType(MessageType.UnsubAck)
                            .AddData(2)
                            .AddMessageId(MessageId);

        return builder.Build();
    }
}
