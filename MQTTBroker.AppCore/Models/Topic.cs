using MQTTBroker.AppCore.Services.Interfaces;

namespace MQTTBroker.AppCore.Models;

public class Topic
{
    public string Name { get; set; }
    public IList<ITcpConnection> Subscribers { get; set; }
}