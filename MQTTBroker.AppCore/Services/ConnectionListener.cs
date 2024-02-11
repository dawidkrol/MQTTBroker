using System.Net;
using System.Net.Sockets;
using MQTTBroker.AppCore.Services.Interface;

namespace MQTTBroker.AppCore.Services;

public class ConnectionListener
{
    private readonly bool _shouldListen;
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
                //addNewClient?.Invoke(new Subscriber(tcpClient, broker, removeClient));
            }
        }
    }
}