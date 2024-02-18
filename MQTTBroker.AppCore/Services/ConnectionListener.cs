using System.Net;
using System.Net.Sockets;
using MQTTBroker.AppCore.Commands.RequestCommands;
using MQTTBroker.AppCore.Services.Interfaces;

namespace MQTTBroker.AppCore.Services;

public class ConnectionListener : IDisposable, IConnectionListener
{
    private bool _shouldListen;
    private readonly TcpListener _listener;
    private readonly int _port;
    private readonly string _ipAddress;
    private IBroker _broker;

    public ConnectionListener(string ipAddress, int port, IBroker broker)
    {
        _port = port;
        _ipAddress = ipAddress;
        _listener = new TcpListener(IPAddress.Parse(_ipAddress), _port);
        _shouldListen = true;
        _broker = broker;
    }

    private TcpClient GotConnection()
    {
        return _listener.AcceptTcpClient();
    }

    public void StartListening()
    {
        Task.Run(
            () =>
            {
                _listener.Start();
                while (_shouldListen)
                {
                    if (GotConnection() is { } tcpClient)
                    {
                        _broker.AddCommandToQueue(new CreateTcpConnectionCommand(tcpClient));
                    }
                }
                _listener.Stop();
        });
    }

    public void StopListening()
    {
        _shouldListen = false;
    }

    public void Dispose() => StopListening();
}