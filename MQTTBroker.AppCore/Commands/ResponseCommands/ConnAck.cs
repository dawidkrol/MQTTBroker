using MQTTBroker.AppCore.Enums;

namespace MQTTBroker.AppCore.Commands.ResponseCommands;

public class ConnAck : ICommand
{
    public ConnackReturnCode ReturnCode { get; private set; }

    public ConnAck(ConnackReturnCode returnCode)
    {
        ReturnCode = returnCode;
    }

    public byte[] ToBuffer()
    {
        byte[] buffer =
        [
            Convert.ToByte("00100000", 2), // CONNACK message type
            Convert.ToByte("00000010", 2), // Remaining length
            Convert.ToByte("00000000", 2), // Reserved values. Not used.
            (byte)ReturnCode, // Connect return code
        ];
        return buffer;
    }
}
