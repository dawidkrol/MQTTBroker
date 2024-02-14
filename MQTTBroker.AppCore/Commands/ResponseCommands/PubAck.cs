namespace MQTTBroker.AppCore.Commands.ResponseCommands;

// QoS = 1
public class PubAck
{
    public ushort MessageId { get; private set; }

    public PubAck(ushort messageId)
    {
        MessageId = messageId;
    }

    public byte[] ToBuffer()
    {
        byte[] buffer =
        [
            Convert.ToByte("01000000", 2), // PUBACK message type
            Convert.ToByte("00000010", 2), // Remaining length
            (byte)(MessageId >> 8), // Message ID MSB
            (byte)(MessageId & 0xFF), // Message ID LSB
        ];
        return buffer;
    }
}
