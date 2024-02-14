using MQTTBroker.AppCore.Models;

namespace MQTTBroker.AppCore.Services.Interface;

public interface ICrudTopicService
{
    Topic? GetTopic(string topicName);
}