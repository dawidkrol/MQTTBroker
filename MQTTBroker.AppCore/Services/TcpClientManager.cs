namespace MQTTBroker.AppCore.Services;

using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public class TcpClientManager
{
    private TcpClient _client;
    private NetworkStream _stream;

    public TcpClientManager(TcpClient client)
    {
        _client = client;
        _stream = _client.GetStream();
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
        }
        finally
        {
            await CloseAsync();
        }
    }

    public async Task SendMessageAsync(string message)
    {
        try
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            await _stream.WriteAsync(buffer, 0, buffer.Length);
            await _stream.FlushAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending message: {ex.Message}");
        }
    }

    public async Task CloseAsync()
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
            int bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length);
            if (bytesRead == 0)
            {
                Console.WriteLine("Client disconnected.");
                break;
            }

            string receivedMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Console.WriteLine($"Received message: {receivedMessage}");

            // Process the received message as needed
        }
    }
}
