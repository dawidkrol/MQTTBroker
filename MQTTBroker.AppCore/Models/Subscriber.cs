namespace MQTTBroker.AppCore.Models
{
    public record SubscriberModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}