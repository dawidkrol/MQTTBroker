using System.Net.Sockets;
using System.Text;
using MQTTBroker.AppCore.Commands;
using MQTTBroker.AppCore.Commands.RequestCommands;
using MQTTBroker.AppCore.Services.Interfaces;

namespace MQTTBroker.AppCore.Services;

public class TcpConnection : IDisposable, ITcpConnection
{
    private bool _isConnectionEstablished { get; set; }
    public string ClientId { get; set; }
    private TcpClient _client;
    private NetworkStream _stream;
    private IBroker _broker;

    public TcpConnection(TcpClient client, IBroker broker)
    {
        _client = client;
        _stream = _client.GetStream();
        _broker = broker;
    }

    public void SetClientId(string clientId)
    {
        ClientId = clientId;
    }

    public string GetClientId()
    {
        return ClientId;
    }

    public bool IsConnectionEstablished()
    {
        return _isConnectionEstablished;
    }

    public void SetConnectionEstablished(bool value)
    {
        _isConnectionEstablished = value;
    }

    public async Task StartAsync()
    {
        try
        {
            ReceiveMessagesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            if (IsConnected())
            {
                await StartAsync();
            }
        }
    }

    public async Task SendMessageAsync(byte[] message)
    {
        try
        {
            await _stream.WriteAsync(message, 0, message.Length);
            await _stream.FlushAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending message: {ex}");
        }
    }

    public void Close()
    {
        try
        {
            if (IsConnected())
            {
                _stream.Close();
                _client.Close();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error closing connection: {ex.Message}");
        }
    }

    public bool IsConnected()
    {
        return _client.Connected;
    }

    private void ReceiveMessagesAsync()
    {
        Task.Run(async () =>
            {
                while (IsConnected())
                {
                    byte[] buffer = new byte[1024];
                    int bytesRead = await _stream.ReadAsync(buffer);
                    if (bytesRead == 0)
                    {
                        break;
                    }

                    string receivedMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    // Console.WriteLine($"Received message: {BitConverter.ToString(buffer)}");

                    try
                    {
                        var command = CommandFactory.CreateCommand(buffer, this);
                        if (command != null)
                        {
                            _broker.AddCommandToQueue(command);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error creating command: {ex}");
                    }
                    finally
                    {
                        _stream.Flush();
                    }
                }
                _broker.AddCommandToQueue(new RemoveDisconnectedClientCommand(this));
            }
        );
    }

    public void Dispose()
    {
        Close();
    }
}
