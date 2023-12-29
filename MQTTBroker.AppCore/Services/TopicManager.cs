using MQTTBroker.AppCore.Exceptions;

namespace MQTTBroker.AppCore.Models
{
    public class TopicManager : ICrudTopicService, ISubscribeOperationsService, IMessageSender
    {
        private List<TopicModel> _topics { get; set; } = new List<TopicModel>();

        public void AddTopic(TopicModel topicModel)
        {
            _topics.Add(topicModel);
        }

        public void RemoveTopic(int topicId)
        {
            var itemToRemove = _topics.SingleOrDefault(x => x.Id == topicId) ?? throw new NotFoundException($"Cannot find topic witch id = {topicId}");
            _topics.Remove(itemToRemove);
        }

        public TopicModel GetTopic(int topicId)
        {
            return _topics.SingleOrDefault(x => x.Id == topicId) ?? throw new NotFoundException($"Cannot find topic witch id = {topicId}");
        }

        public void SubscribeTopic(int topicId, SubscriberModel subscriber)
        {
            var topicToSubscribe = _topics.SingleOrDefault(x => x.Id == topicId) ?? throw new NotFoundException($"Cannot find topic witch id = {topicId}");
            topicToSubscribe.Subscribers.Append(subscriber);
        }

        public void UnsubscribeTopic(int topicId, Guid subscriberId)
        {
            var topicToUnsubscribe = _topics.SingleOrDefault(x => x.Id == topicId) ?? throw new NotFoundException($"Cannot find topic witch id = {topicId}");
            var subscriber = topicToUnsubscribe.Subscribers.SingleOrDefault(x => x.Id == subscriberId) ?? throw new NotFoundException($"Cannot find subscriber witch id = {subscriberId} in topic witch id = {topicId}");
            topicToUnsubscribe.Subscribers.Remove(subscriber);
        }

        public void SendmMssageToSubscribers(int topicId, string message)
        {
            var topic = this._topics.SingleOrDefault(x => x.Id == topicId) ?? throw new NotFoundException($"Cannot find topic witch id = {topicId}");
            foreach (var subscriber in topic.Subscribers)
            {
                // TODO: Sending messages
            }
        }
    }
}
