namespace MQTTBroker.AppCore.Commands.ResponseCommands;

public class PingResp
{
    public byte[] ToBuffer()
    {
        byte[] buffer =
        [
            Convert.ToByte("11010000", 2), // PINGRESP message type
            Convert.ToByte("00000000", 2), // Remaining length
        ];
        return buffer;
    }
}
