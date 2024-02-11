using MQTTBroker.AppCore.Models;

namespace MQTTBroker.AppCore.Services.Interface;

public interface ICrudTopicService
{
    void AddTopic(TopicModel topicModel);

    void RemoveTopic(int topicId);

    TopicModel GetTopic(int topicId);
}