using System.Text.RegularExpressions;
using MQTTBroker.AppCore.Commands.RequestCommands;
using MQTTBroker.AppCore.Commands.ResponseCommands;
using MQTTBroker.AppCore.Filters;
using MQTTBroker.AppCore.Models;
using MQTTBroker.AppCore.Services.Interfaces;

namespace MQTTBroker.AppCore.Services;

public class TopicManager : ITopicManager
{
    private readonly IBroker _broker;
    private List<Topic> Topics { get; set; } = new();

    public TopicManager(IBroker broker)
    {
        _broker = broker;
    }

    public Topic? GetTopic(string topicName)
    {
        return Topics.SingleOrDefault(x => x.Pattern == topicName);
    }

    public async Task SubscribeTopic(SubscribeCommand subscribeCommand)
    {
        if (subscribeCommand.TopicName.Contains("+") &&
            !new Regex(@"^((\+\/)?[a-zA-Z0-9]+(\/(\+|[a-zA-Z0-9]+))*)|\+$")
                .IsMatch(subscribeCommand.TopicName))
        {
            Console.WriteLine($"Wrong topic name, cannot subscribe to {subscribeCommand.TopicName}");
            return;
        }

        if (subscribeCommand.TopicName.Contains("#") && !new Regex(@"(^#$)|(/#$)")
                .IsMatch(subscribeCommand.TopicName))
        {
            Console.WriteLine($"Wrong topic name, cannot subscribe to {subscribeCommand.TopicName}");
            return;
        }

        var topicToSubscribe = Topics.SingleOrDefault(x => x.Pattern == subscribeCommand.TopicName);
        if (topicToSubscribe == null)
        {
            topicToSubscribe = new Topic
            {
                Pattern = subscribeCommand.TopicName,
                Subscribers = new List<ITcpConnection>()
            };
            Topics.Add(topicToSubscribe);
        }
        topicToSubscribe.Subscribers.Add(subscribeCommand.TcpConnection);

        await Console.Out.WriteLineAsync($"Topic {topicToSubscribe.Pattern} subscribed");

        await _broker.SendResponse(new SubAck(subscribeCommand.MessageId, new List<byte>{0}), subscribeCommand.TcpConnection);
    }

    public async Task UnsubscribeTopic(UnsubscribeCommand unsubscribeCommand)
    {
        var result = RemoveTopicSubscribtion(unsubscribeCommand.TopicName, unsubscribeCommand.TcpConnection);
        if (!result) return;
        await _broker.SendResponse(new UnsubAck(unsubscribeCommand.MessageId), unsubscribeCommand.TcpConnection);
        await Console.Out.WriteLineAsync($"Topic {unsubscribeCommand.TopicName} unsubscribed");
    }

    public void RemoveTcpConnection(DisconnectCommand disconnectCommand)
    {
        foreach (var topic in Topics)
        {
            RemoveTopicSubscribtion(topic.Pattern, disconnectCommand.TcpConnection);
        }
        Topics.RemoveAll(x => x.Subscribers.Count == 0);
        Console.WriteLine("Removed tcp connection");
    }
    
    public void RemoveTcpConnection(RemoveDisconnectedClientCommand removeDisconnectedClientCommand)
    {
        foreach (var topic in Topics)
        {
            RemoveTopicSubscribtion(topic.Pattern, removeDisconnectedClientCommand.TcpConnection);
        }
        Topics.RemoveAll(x => x.Subscribers.Count == 0);
        Console.WriteLine("Removed tcp connection");
    }

    public async Task PublishMessage(PublishCommand publishCommand)
    {
        if (publishCommand.TopicName.Contains("+") || publishCommand.TopicName.Contains("#") ||
            publishCommand.TopicName.Contains("$") || publishCommand.TopicName.Contains("*"))
        {
            Console.WriteLine($"Wrong topic name, cannot publish on {publishCommand.TopicName}");
            return;
        }

        var topics = Topics
            .Where(x => FilterTopicsComparer.Compare(publishCommand.TopicName, x.Pattern));

        foreach (var topic in topics)
        {
            foreach (var subscriber in topic.Subscribers)
            {
                try
                {
                    await _broker.SendResponse(publishCommand, subscriber);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending message: {ex}");
                }
            }
        }

        await Console.Out.WriteLineAsync("Message has been published");
        await _broker.SendResponse(new PubAck(publishCommand.MessageId), publishCommand.TcpConnection);
    }

    private bool RemoveTopicSubscribtion(string topicName, ITcpConnection tcpConnection)
    {
        var topicToUnsubscribe = Topics.SingleOrDefault(x => x.Pattern == topicName);
        if (topicToUnsubscribe == null)
        {
            Console.WriteLine("No topic to unsubscribe");
            return false;
        }
        
        topicToUnsubscribe.Subscribers.Remove(tcpConnection);
        return true;
    }
}