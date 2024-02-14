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
        byte[] buffer =
        [
            Convert.ToByte("10110000", 2), // UNSUBACK message type
            Convert.ToByte("00000010", 2), // Remaining length
            (byte)(MessageId >> 8), // Message ID MSB
            (byte)(MessageId & 0xFF), // Message ID LSB
        ];
        return buffer;
    }
}
