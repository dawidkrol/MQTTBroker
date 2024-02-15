using MQTTBroker.AppCore.Enums;

namespace MQTTBroker.AppCore.Commands.Builder;

public class CommandBuilder : ICommandBuilder
{
    private byte[] _command = new byte[10];
    private int _pointer = 0;
    private readonly int _lenOfCommand;

    public static CommandBuilder TotalLenghOfCommand(int lenOfCommand)
    {
        return new CommandBuilder(lenOfCommand);
    }

    private CommandBuilder(int lenOfCommand)
    {
        _lenOfCommand = lenOfCommand;
        Reset();
    }

    public void Reset()
    {
        _command = new byte[_lenOfCommand];
    }

    public ICommandBuilder AddCommandType(MessageType messageType)
    {
        _command[_pointer] = (byte)((byte)messageType << 4);
        setPointer();
        return this;
    }

    public ICommandBuilder AddData(int length)
    {
        _command[_pointer] = (byte)length;
        setPointer();
        return this;
    }

    public ICommandBuilder AddEmptyLine()
    {
        _command[_pointer] = Convert.ToByte("00000000", 2);
        setPointer();
        return this;
    }

    public ICommandBuilder AddConnectReturnCode(ConnackReturnCode returnCode)
    {
        _command[_pointer] = (byte)returnCode;
        setPointer();
        return this;
    }

    public ICommandBuilder AddMessageId(int messageId)
    {
        _command[_pointer] = (byte)(messageId >> 8);
        setPointer();

        _command[_pointer] = (byte)(messageId & 0xFF);
        setPointer();

        return this;
    }

    public byte[] Build()
    {
        return _command;
    }

    private void setPointer()
    {
        _pointer += 1;
    }
}
