namespace MQTTBroker.AppCore.Models
{
    public interface ICrudTopicService
    {
        void AddTopic(TopicModel topicModel);

        void RemoveTopic(int topicId);

        TopicModel GetTopic(int topicId);
    }
}