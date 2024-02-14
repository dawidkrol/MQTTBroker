using System.Text;

namespace MQTTBroker.AppCore.Commands.ResponseCommands;

public class SubAck
{
    public ushort MessageId { get; private set; }
    public List<byte> GrantedQosLevels { get; private set; }

    public SubAck(ushort messageId, List<byte> grantedQosLevels)
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
                                .AddMessageId(MessageId)
                                .AddData(GrantedQosLevels[0]);

        for (int i = 0; i < GrantedQosLevels.Count; i++)
        {
            builder.AddData(GrantedQosLevels[i]);
        }

        return builder.Build();
    }
}
