namespace MQTTBroker.AppCore.Models
{
    public record TopicModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<SubscriberModel> Subscribers { get; set; }
    }
}