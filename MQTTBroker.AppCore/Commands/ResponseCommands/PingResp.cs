namespace MQTTBroker.AppCore.Commands.ResponseCommands;

public class PingResp
{
    public byte[] ToBuffer()
    {
        var builder = CommandBuilder
                            .TotalLenghOfCommand(2)
                                .AddCommandType(Enums.MessageType.PingResp)
                                .AddEmptyLine();
                                
        return builder.Build();
    }
}
