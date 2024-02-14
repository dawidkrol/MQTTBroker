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

        var builder = CommandBuilder
                            .TotalLenghOfCommand(4)
                                .AddCommandType(MessageType.ConnAck)
                                .AddData(2)
                                .AddEmptyLine()
                                .AddConnectReturnCode(ReturnCode);
                                
        return builder.Build();
    }
}
