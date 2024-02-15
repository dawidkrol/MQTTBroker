using MQTTBroker.AppCore.Commands.RequestCommands;
using MQTTBroker.AppCore.Commands.ResponseCommands;
using MQTTBroker.AppCore.Exceptions;
using MQTTBroker.AppCore.Models;
using MQTTBroker.AppCore.Services.Interface;
using MQTTBroker.AppCore.Services.Interfaces;

namespace MQTTBroker.AppCore.Services;

public class TopicManager : ITopicManager
{
    private readonly IBroker _broker;
    private List<Topic> Topics { get; set; } = [];

    public TopicManager(IBroker broker)
    {
        _broker = broker;
    }

    public Topic? GetTopic(string topicName)
    {
        return Topics.SingleOrDefault(x => x.Name == topicName);
    }

    public async Task SubscribeTopic(SubscribeCommand subscribeCommand)
    {
        var topicToSubscribe = Topics.SingleOrDefault(x => x.Name == subscribeCommand.TopicName);
        if (topicToSubscribe == null)
        {
            topicToSubscribe = new Topic
            {
                Name = subscribeCommand.TopicName,
                Subscribers = []
            };
            Topics.Add(topicToSubscribe);
        }
        topicToSubscribe.Subscribers.Add(subscribeCommand.TcpConnection);

        await _broker.SendResponce(new SubAck(subscribeCommand.MessageId, [0, 1]), subscribeCommand.TcpConnection);
    }

    public async Task UnsubscribeTopic(UnsubscribeCommand unsubscribeCommand)
    {
        RemoveTopicSubscribtion(unsubscribeCommand.TopicName, unsubscribeCommand.TcpConnection);
        await _broker.SendResponce(new UnsubAck(unsubscribeCommand.MessageId), unsubscribeCommand.TcpConnection);
    }

    public void RemoveTcpConnection(DisconnectCommand disconnectCommand)
    {
        foreach (var topic in Topics)
        {
            RemoveTopicSubscribtion(topic.Name, disconnectCommand.TcpConnection);
        }
    }

    public async Task PublishMessage(PublishCommand publishCommand)
    {
        var topic = Topics.SingleOrDefault(x => x.Name == publishCommand.TopicName) ?? throw new NotFoundException($"Cannot find topic witch name = {publishCommand.TopicName}");
        foreach (var subscriber in topic.Subscribers)
        {
            // potrzebujemy czekać na wysłanie przed odesłaniem ack?
            subscriber.SendMessageAsync(publishCommand.Payload);
        }
        await _broker.SendResponce(new PubAck(publishCommand.MessageId), publishCommand.TcpConnection);
    }

    private void RemoveTopicSubscribtion(string topicName, ITcpConnection tcpConnection)
    {
        var topicToUnsubscribe = Topics.SingleOrDefault(x => x.Name == topicName) ?? throw new NotFoundException($"Cannot find topic witch name = {topicName}");
        topicToUnsubscribe.Subscribers.Remove(tcpConnection);

        if (topicToUnsubscribe.Subscribers.Count == 0)
        {
            Topics.Remove(topicToUnsubscribe);
        }
    }
}