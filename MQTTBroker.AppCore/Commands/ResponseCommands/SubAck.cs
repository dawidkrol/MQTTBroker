using MQTTBroker.AppCore.Commands.Builder;
using MQTTBroker.AppCore.Enums;

namespace MQTTBroker.AppCore.Commands.ResponseCommands;

public class SubAck : IResponseCommand
{
    public int MessageId { get; private set; }
    public List<byte> GrantedQosLevels { get; private set; } = [0];

    public SubAck(int messageId, List<byte> grantedQosLevels)
    {
        MessageId = messageId;
        GrantedQosLevels = grantedQosLevels;
    }

    public byte[] ToBuffer()
    {
        var builder = CommandBuilder
                        .TotalLenghOfCommand(GrantedQosLevels.Count + 4)
                        .AddCommandType(MessageType.SubAck)
                        .AddData(GrantedQosLevels.Count + 2)
                        .AddMessageId(MessageId);

        for (int i = 0; i < GrantedQosLevels.Count; i++)
        {
            builder.AddData(GrantedQosLevels[i]);
        }

        return builder.Build();
    }
}
