using System.Text;
using MQTTBroker.AppCore.Services.Interfaces;

namespace MQTTBroker.AppCore.Commands.RequestCommands;

public class ConnectCommand : ICommand
{
    private readonly byte[] _data;
    public ITcpConnection TcpConnection { get; }
    public string ClientId { get; set; }
    public int Version { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }

    private const string ProtocolName = "MQIsdp";
    private const ushort KeepAliveTimer = 120;

    public ConnectCommand(byte[] data, ITcpConnection tcpConnection)
    {
        _data = data;
        TcpConnection = tcpConnection;
        ExtractData();
    }

    void ExtractData()
    {
        Version = _data[6];
        if (Version != 3 && Version != 4)
        {
            return;
        }
        var clientIdLength = (_data[10] << 8) | _data[11];
        ClientId = Encoding.UTF8.GetString(_data[12..(clientIdLength + 12)]);

        var usernameAndPasswordStartIndex = 12 + clientIdLength;
        // check if the username flag is set to 1
        if ((_data[7] & 0b1000_0000) == 0b1000_0000)
        {
            var usernameLength = (_data[usernameAndPasswordStartIndex] << 8) | _data[usernameAndPasswordStartIndex + 1];
            Username = Encoding.UTF8.GetString(_data[(usernameAndPasswordStartIndex + 2)..(usernameAndPasswordStartIndex + 2 + usernameLength)]);
            // Update the starting index for the password
            usernameAndPasswordStartIndex += 2 + usernameLength;
        }
        // check if the password flag is set to 1
        if ((_data[7] & 0b0100_0000) == 0b0100_0000)
        {
            var passwordLength = (_data[usernameAndPasswordStartIndex] << 8) | _data[usernameAndPasswordStartIndex + 1];
            Password = Encoding.UTF8.GetString(_data[(usernameAndPasswordStartIndex + 2)..(usernameAndPasswordStartIndex + 2 + passwordLength)]);
        }
    }
}