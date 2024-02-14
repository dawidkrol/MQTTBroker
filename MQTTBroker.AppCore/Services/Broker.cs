using MQTTBroker.AppCore.Commands;
using MQTTBroker.AppCore.Commands.RequestCommands;
using MQTTBroker.AppCore.Commands.ResponseCommands;
using MQTTBroker.AppCore.Services.Interface;
using System.Collections.Concurrent;

namespace MQTTBroker.AppCore.Services;

public class Broker : IBroker
{
    private static Broker instance;
    public readonly string _host = "localhost";
    public readonly int _port = 1883;
    private readonly ConnectionListener _connectionListener;
    private readonly TopicManager _topicManager;
    private readonly ClientManager _clientManager;
    private readonly ConcurrentQueue<ICommand> Commands = new();
    private CancellationTokenSource _cts = new();
    private Task _commandExecutionTask;

    public static IBroker GetBroker()
    {
        instance ??= new Broker();
        return instance;
    }

    private Broker()
    {
        _connectionListener = new ConnectionListener(_host, _port);
        _commandExecutionTask = Task.Run(() => ExecuteCommands(_cts.Token));
    }

    public void Start()
    {
        _connectionListener.StartListening(this);
    }

    public void AddCommandToQueue(ICommand command)
    {
        Commands.Enqueue(command);
    }

    private async Task ExecuteCommands(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            if (Commands.TryDequeue(out ICommand command))
            {
                await ExecuteCommand(command);
            }
            else
            {
                await Task.Delay(100, cancellationToken); // wait for a while before checking the queue again
            }
        }
    }

    public void Stop()
    {
        _cts.Cancel();
        _commandExecutionTask.Wait();
    }

    private async Task ExecuteCommand(ICommand command)
    {
        switch (command)
        {
            case CreateTcpConnectionCommand createTcpConnectionCommand:
                await _clientManager.AddTcpConnection(createTcpConnectionCommand.TcpClient, this);
                break;
            case ConnectCommand connectCommand:
                _clientManager.ChangeConnectionStatus(connectCommand.TcpConnection, true);
                await connectCommand.TcpConnection.SendMessageAsync(new ConnAck(Enums.ConnackReturnCode.ConnectionAccepted).ToBuffer());
                break;
            case DisconnectCommand disconnectCommand:
                _topicManager.RemoveTcpConnection(disconnectCommand.TcpConnection);
                break;
            case SubscribeCommand subscribeCommand:
                _topicManager.SubscribeTopic(subscribeCommand.TopicName, subscribeCommand.TcpConnection);
                await subscribeCommand.TcpConnection.SendMessageAsync(new SubAck(subscribeCommand.MessageId, [0, 1]).ToBuffer());
                break;
            case UnsubscribeCommand unsubscribeCommand:
                _topicManager.UnsubscribeTopic(unsubscribeCommand.TopicName, unsubscribeCommand.TcpConnection);
                await unsubscribeCommand.TcpConnection.SendMessageAsync(new UnsubAck(unsubscribeCommand.MessageId).ToBuffer());
                break;
            case PublishCommand publishCommand:
                _topicManager.PublishMessage(publishCommand.TopicName, publishCommand.Payload);
                // For QoS = 1
                await publishCommand.TcpConnection.SendMessageAsync(new PubAck(publishCommand.MessageId).ToBuffer());
                break;
            default:
                throw new NotImplementedException();
        }
    }
}