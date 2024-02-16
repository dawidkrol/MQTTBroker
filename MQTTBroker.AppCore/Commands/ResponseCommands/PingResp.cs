using MQTTBroker.AppCore.Commands.Builder;
using MQTTBroker.AppCore.Enums;

namespace MQTTBroker.AppCore.Commands.ResponseCommands;

public class PingResp : IResponseCommand
{
    public byte[] ToBuffer()
    {
        var builder = CommandBuilder
                            .TotalLenghOfCommand(2)
                                .AddCommandType(MessageType.PingResp)
                                .AddEmptyLine();
                                
        return builder.Build();
    }
}
