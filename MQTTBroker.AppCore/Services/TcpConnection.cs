namespace MQTTBroker.AppCore.Services;

using MQTTBroker.AppCore.Commands;
using MQTTBroker.AppCore.Services.Interface;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public class TcpConnection
{
    public bool IsConnectionEstablished { get; set; }
    private TcpClient _client;
    private NetworkStream _stream;
    private IBroker _broker;
    // private Queue<string> _messageQueue = new Queue<string>();

    public TcpConnection(TcpClient client, IBroker broker)
    {
        _client = client;
        _stream = _client.GetStream();
        _broker = broker;
    }

    public async Task StartAsync()
    {
        try
        {
            await ReceiveMessagesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            if (IsConnected())
            {
                await StartAsync();
            }
        }
        finally
        {
            Close();
        }
    }

    public async Task SendMessageAsync(byte[] message)
    {
        // if (_messageQueue.TryDequeue(out string message))
        try
        {
            await _stream.WriteAsync(message, 0, message.Length);
            await _stream.FlushAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending message: {ex.Message}");
        }
    }

    public void Close()
    {
        try
        {
            if (_client.Connected)
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

    private async Task ReceiveMessagesAsync()
    {
        byte[] buffer = new byte[1024];
        while (_client.Connected)
        {
            int bytesRead = await _stream.ReadAsync(buffer);
            if (bytesRead == 0)
            {
                Console.WriteLine("Client disconnected.");
                break;
            }

            string receivedMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Console.WriteLine($"Received message: {receivedMessage}");

            var command = CommandFactory.CreateCommand(buffer, this);
            _broker.AddCommandToQueue(command);
        }
    }
}
