using MQTTBroker.AppCore.Services;

namespace MQTTBroker.AppCore.Models
{
    public class Topic
    {
        public string Name { get; set; }
        public IList<TcpConnection> Subscribers { get; set; }
    }
}