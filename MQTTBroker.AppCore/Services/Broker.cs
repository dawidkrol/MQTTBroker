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
            default:
                throw new NotImplementedException();
        }
    }
}