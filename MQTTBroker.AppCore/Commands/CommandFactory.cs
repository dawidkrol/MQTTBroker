using MQTTBroker.AppCore.Commands.RequestCommands;
using MQTTBroker.AppCore.Enums;
using MQTTBroker.AppCore.Services.Interfaces;

namespace MQTTBroker.AppCore.Commands;

public static class CommandFactory
{
    public static ICommand CreateCommand(byte[] data, ITcpConnection tcpConnection)
    {
        var messageType = (MessageType)(data[0] << 4);
        var remainingLength = 0;
        var multiplier = 1;
        var currentByte = 1;
        byte digit;

        do
        {
            digit = data[currentByte++];
            remainingLength += (digit & 127) * multiplier;
            multiplier *= 128;

        } while ((digit & 128) != 0);
        
        var message = data[2..(remainingLength + 2)];

        return messageType switch
        {
            MessageType.Connect => new ConnectCommand(message, tcpConnection),
            MessageType.Publish => new PublishCommand(message, tcpConnection),
            MessageType.Subscribe => new SubscribeCommand(message, tcpConnection),
            MessageType.Unsubscribe => new UnsubscribeCommand(message, tcpConnection),
            MessageType.PingReq => new PingReqCommand(tcpConnection),
            MessageType.Disconnect => new DisconnectCommand(tcpConnection),
            _ => throw new NotImplementedException()
        };
    }
}