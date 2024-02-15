using System.Text;
using MQTTBroker.AppCore.Commands.Builder;

namespace MQTTBroker.AppCore.Commands.ResponseCommands;

public class SubAck : IResponceCommand
{
    public int MessageId { get; private set; }
    public List<byte> GrantedQosLevels { get; private set; }

    public SubAck(int messageId, List<byte> grantedQosLevels)
    {
        MessageId = messageId;
        GrantedQosLevels = grantedQosLevels;
    }

    public byte[] ToBuffer()
    {
        var builder = CommandBuilder
                            .TotalLenghOfCommand(GrantedQosLevels.Count + 4)
                                .AddCommandType(Enums.MessageType.SubAck)
                                .AddData(GrantedQosLevels.Count)
                                .AddMessageId(MessageId);

        for (int i = 0; i < GrantedQosLevels.Count; i++)
        {
            builder.AddData(GrantedQosLevels[i]);
        }

        return builder.Build();
    }
}
