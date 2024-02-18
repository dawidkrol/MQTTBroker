using MQTTBroker.AppCore.Commands.RequestCommands;
using MQTTBroker.AppCore.Services.Interfaces;

namespace MQTTBroker.AppCore.Models;

public class Topic
{
    public string Pattern { get; set; }
    public IList<ITcpConnection> Subscribers { get; set; }

    public void Subscribe(ITcpConnection tcpConnection)
    {
        Subscribers.Add(tcpConnection);
    }

    public void Unsubscribe(ITcpConnection tcpConnection)
    {
        Subscribers.Remove(tcpConnection);
    }

    public void Publish(PublishCommand publishCommand, IBroker broker) 
    {
        foreach (var subscriber in Subscribers)
        {
            broker.SendResponse(publishCommand, subscriber);
        }
    }
}