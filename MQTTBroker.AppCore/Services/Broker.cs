using MQTTBroker.AppCore.Models;
using MQTTBroker.AppCore.Services.Interface;

namespace MQTTBroker.AppCore.Services;

public class Broker : IBroker
{
    private readonly ConnectionListener _connectionListener;
    private readonly TopicManager _topicManager;
    private readonly TcpClientManager _tcpClientManager;

    public Broker(string host, int port)
    {
        _connectionListener = new ConnectionListener(host, port);
    }

    public void Start()
    {
        _connectionListener.StartListening(this, AddNewClient, RemoveClient);
    }

    private void RemoveClient(SubscriberModel client)
    {
    }

    private void AddNewClient(SubscriberModel client)
    {
    }

    public void NewSubscribtion() { }

    public void RemoveSubscription() { }

    public void Publish() { }
}