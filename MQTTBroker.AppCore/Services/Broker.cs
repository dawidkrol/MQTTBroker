using MQTTBroker.AppCore.Commands;
using MQTTBroker.AppCore.Services.Interface;

namespace MQTTBroker.AppCore.Services;

public class Broker : IBroker
{
    private readonly ConnectionListener _connectionListener;
    private readonly TopicManager _topicManager;
    private readonly ClientManager _clientManager;

    public Broker(string host, int port)
    {
        _connectionListener = new ConnectionListener(host, port);
    }

    public void Start()
    {
        _connectionListener.StartListening(this);
    }

    public void ExecuteCommand(ICommand command)
    {
        switch (command)
        {
            case ConnectCommand connectCommand:
                _clientManager.AddTcpConnection(connectCommand.Client);
                break;
            case DisconnectCommand disconnectCommand:
                _topicManager.RemoveTcpConnection(disconnectCommand.TcpConnection);
                break;
            case SubscribeCommand subscribeCommand:
                _topicManager.SubscribeTopic(subscribeCommand.TopicName, subscribeCommand.TcpConnection);
                break;
            case UnsubscribeCommand unsubscribeCommand:
                _topicManager.UnsubscribeTopic(unsubscribeCommand.TopicName, unsubscribeCommand.TcpConnection);
                break;
            case PublishCommand publishCommand:
                _topicManager.PublishMessage(publishCommand.TopicName, publishCommand.Message);
                break;
            default:
                throw new NotImplementedException();
        }
    }
}