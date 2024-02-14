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
        byte[] buffer = new byte[2 + GrantedQosLevels.Count];
        buffer[0] = Convert.ToByte("10010000", 2); // SUBACK message type
        buffer[1] = (byte)GrantedQosLevels.Count; // Remaining length
        buffer[2] = (byte)(MessageId >> 8); // Message ID MSB
        buffer[3] = (byte)(MessageId & 0xFF); // Message ID LSB
        for (int i = 0; i < GrantedQosLevels.Count; i++)
        {
            buffer[4 + i] = GrantedQosLevels[i]; // Granted QoS levels
        }
        return buffer;
    }
}
