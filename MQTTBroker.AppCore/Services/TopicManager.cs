using MQTTBroker.AppCore.Exceptions;
using MQTTBroker.AppCore.Models;
using MQTTBroker.AppCore.Services.Interface;

namespace MQTTBroker.AppCore.Services;

public class TopicManager : ICrudTopicService, ISubscribeOperationsService, IMessageSender
{
    private List<TopicModel> Topics { get; set; } = new List<TopicModel>();

    public void AddTopic(TopicModel topicModel)
    {
        Topics.Add(topicModel);
    }

    public void RemoveTopic(int topicId)
    {
        var itemToRemove = Topics.SingleOrDefault(x => x.Id == topicId) ?? throw new NotFoundException($"Cannot find topic witch id = {topicId}");
        Topics.Remove(itemToRemove);
    }

    public TopicModel GetTopic(int topicId)
    {
        return Topics.SingleOrDefault(x => x.Id == topicId) ?? throw new NotFoundException($"Cannot find topic witch id = {topicId}");
    }

    public void SubscribeTopic(int topicId, SubscriberModel subscriber)
    {
        var topicToSubscribe = Topics.SingleOrDefault(x => x.Id == topicId) ?? throw new NotFoundException($"Cannot find topic witch id = {topicId}");
        topicToSubscribe.Subscribers.Add(subscriber);
    }

    public void UnsubscribeTopic(int topicId, Guid subscriberId)
    {
        var topicToUnsubscribe = Topics.SingleOrDefault(x => x.Id == topicId) ?? throw new NotFoundException($"Cannot find topic witch id = {topicId}");
        var subscriber = topicToUnsubscribe.Subscribers.SingleOrDefault(x => x.Id == subscriberId) ?? throw new NotFoundException($"Cannot find subscriber witch id = {subscriberId} in topic witch id = {topicId}");
        topicToUnsubscribe.Subscribers.Remove(subscriber);
    }

    public void SendMessageToSubscribers(int topicId, string message)
    {
        var topic = Topics.SingleOrDefault(x => x.Id == topicId) ?? throw new NotFoundException($"Cannot find topic witch id = {topicId}");
        foreach (var subscriber in topic.Subscribers)
        {
            // TODO: Sending messages
        }
    }
}