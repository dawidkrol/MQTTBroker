namespace MQTTBroker.Broker
{
    public class TopicModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Subscriber> Topic { get; set; }
    }
}