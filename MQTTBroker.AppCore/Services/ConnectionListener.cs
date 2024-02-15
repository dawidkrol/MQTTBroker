using System.Net;
using System.Net.Sockets;
using MQTTBroker.AppCore.Commands.RequestCommands;
using MQTTBroker.AppCore.Services.Interface;
using MQTTBroker.AppCore.Services.Interfaces;

namespace MQTTBroker.AppCore.Services;

public class ConnectionListener : IDisposable, IConnectionListener
{
    private bool _shouldListen;
    private readonly TcpListener _listener;
    private readonly int _port;
    private readonly string _ipAddress;

    public ConnectionListener(string ipAddress, int port)
    {
        _port = port;
        _ipAddress = ipAddress;
        _listener = new TcpListener(IPAddress.Parse(_ipAddress), _port);
        _shouldListen = true;
    }

    private TcpClient GotConnection()
    {
        return _listener.AcceptTcpClient();
    }

    public void StartListening(IBroker broker)
    {
        _listener.Start();
        while (_shouldListen)
        {
            if (GotConnection() is { } tcpClient)
            {
                broker.AddCommandToQueue(new CreateTcpConnectionCommand(tcpClient));
            }
        }
        _listener.Stop();
    }

    public void StopListening()
    {
        _shouldListen = false;
    }

    public void Dispose() => StopListening();
}