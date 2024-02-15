using MQTTBroker.AppCore.Enums;

namespace MQTTBroker.AppCore.Commands.Builder
{
    public interface ICommandBuilder
    {
        ICommandBuilder AddCommandType(MessageType messageType);
        ICommandBuilder AddData(int data);
        ICommandBuilder AddEmptyLine();
        ICommandBuilder AddConnectReturnCode(ConnackReturnCode returnCode);
        ICommandBuilder AddMessageId(int messageId);
        byte[] Build();
    }
}