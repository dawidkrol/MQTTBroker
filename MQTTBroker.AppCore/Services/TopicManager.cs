using MQTTBroker.AppCore.Exceptions;
using MQTTBroker.AppCore.Models;
using MQTTBroker.AppCore.Services.Interface;

namespace MQTTBroker.AppCore.Services;

public class TopicManager : ICrudTopicService, ISubscribeOperationsService, IMessageSender
{
    private List<Topic> Topics { get; set; } = new();

    public Topic? GetTopic(string topicName)
    {
        return Topics.SingleOrDefault(x => x.Name == topicName);
    }

    public void SubscribeTopic(string topicName, TcpConnection subscriber)
    {
        var topicToSubscribe = Topics.SingleOrDefault(x => x.Name == topicName);
        if (topicToSubscribe == null)
        {
            topicToSubscribe = new Topic
            {
                Name = topicName,
                Subscribers = new List<TcpConnection>()
            };
            Topics.Add(topicToSubscribe);
        }
        topicToSubscribe.Subscribers.Add(subscriber);
    }

    public void UnsubscribeTopic(string name, TcpConnection tcpConnection)
    {
        var topicToUnsubscribe = Topics.SingleOrDefault(x => x.Name == name) ?? throw new NotFoundException($"Cannot find topic witch name = {name}");
        topicToUnsubscribe.Subscribers.Remove(tcpConnection);
        
        if (topicToUnsubscribe.Subscribers.Count == 0)
        {
            Topics.Remove(topicToUnsubscribe);
        }
    }
    
    public void RemoveTcpConnection(TcpConnection tcpConnection)
    {
        foreach (var topic in Topics)
        {
            UnsubscribeTopic(topic.Name, tcpConnection);
        }
    }

    public void PublishMessage(string topicName, byte[] message)
    {
        var topic = Topics.SingleOrDefault(x => x.Name == topicName) ?? throw new NotFoundException($"Cannot find topic witch name = {topicName}");
        foreach (var subscriber in topic.Subscribers)
        {
            subscriber.SendMessageAsync(message);
        }
    }
}