using MQTTBroker.AppCore.Commands.RequestCommands;
using MQTTBroker.AppCore.Enums;
using MQTTBroker.AppCore.Services.Interfaces;

namespace MQTTBroker.AppCore.Commands;

public static class CommandFactory
{
    public static ICommand CreateCommand(byte[] data, ITcpConnection tcpConnection)
    {
        var messageType = (MessageType)(data[0] >> 4);
        Console.WriteLine(messageType.ToString());
        var remainingLength = 0;
        var multiplier = 1;
        var currentByte = 1;
        byte digit;

        byte qos = (byte)(data[0] & 0b0000_0011);

        do
        {
            digit = data[currentByte++];
            remainingLength += (digit & 127) * multiplier;
            multiplier *= 128;

        } while ((digit & 128) != 0);

        //if(qos != 0 && messageType == MessageType.Publish)
        //{
        //    throw new NotImplementedException("Wrong qos");
        //    //messageType = MessageType.PingReq;
        //}
        
        var message = data[2..(remainingLength + 4)];

        Console.WriteLine(remainingLength);

        //if (messageType == MessageType.Publish && (data[0] & 0b0000_0001) == 1)
        //{
        //    throw new NotImplementedException("Wrong qos");
        //}


        //byte messageType_v2 = data[0];
        //Console.WriteLine("Moj typ wiadomoœci: " + messageType);
        //Console.WriteLine("Typ wiadomoœci (w formacie hex): " + messageType_v2.ToString("X"));
        //Console.WriteLine("Typ wiadomoœci (w formacie dec): " + messageType_v2);

        //// Odczytaj d³ugoœæ "remaining length"
        //byte remainingLength_v2 = data[1];
        //Console.WriteLine("Moje 'remaining length': " + remainingLength);
        //Console.WriteLine("D³ugoœæ 'remaining length' (w formacie hex): " + remainingLength_v2.ToString("X"));
        //Console.WriteLine("D³ugoœæ 'remaining length' (w formacie dec): " + remainingLength_v2);

        return messageType switch
        {
            MessageType.Connect => new ConnectCommand(message, tcpConnection),
            MessageType.Publish => new PublishCommand(message, tcpConnection, data),
            MessageType.Subscribe => new SubscribeCommand(message, tcpConnection),
            MessageType.Unsubscribe => new UnsubscribeCommand(message, tcpConnection),
            MessageType.PingReq => new PingReqCommand(tcpConnection),
            MessageType.Disconnect => new DisconnectCommand(tcpConnection),
            _ => throw new NotImplementedException()
        };
    }
}