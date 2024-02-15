using MQTTBroker.AppCore.Commands;
using MQTTBroker.AppCore.Commands.RequestCommands;
using MQTTBroker.AppCore.Commands.ResponseCommands;
using MQTTBroker.AppCore.Services.Interface;
using MQTTBroker.AppCore.Services.Interfaces;
using System.Collections.Concurrent;

namespace MQTTBroker.AppCore.Services;

public class Broker : IBroker
{
    private static Broker _instance;
    public readonly string _host = "localhost";
    public readonly int _port = 1883;
    private readonly IConnectionListener _connectionListener;
    private readonly ITopicManager _topicManager;
    private readonly IClientManager _clientManager;
    private readonly ConcurrentQueue<ICommand> Commands = new();
    private CancellationTokenSource _cts = new();
    private Task _commandExecutionTask;

    public static IBroker GetBroker()
    {
        _instance ??= new Broker();
        return _instance;
    }

    private Broker()
    {
        _connectionListener = new ConnectionListener(_host, _port);
        _clientManager = new ClientManager(this);
        _topicManager = new TopicManager(this);
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
                await _clientManager.AddConnection(createTcpConnectionCommand);
                break;
            case ConnectCommand connectCommand:
                await _clientManager.EstablishConnection(connectCommand);
                break;
            case DisconnectCommand disconnectCommand:
                _topicManager.RemoveTcpConnection(disconnectCommand);
                break;
            case SubscribeCommand subscribeCommand:
                await _topicManager.SubscribeTopic(subscribeCommand);
                break;
            case UnsubscribeCommand unsubscribeCommand:
                await _topicManager.UnsubscribeTopic(unsubscribeCommand);
                break;
            case PublishCommand publishCommand:
                await _topicManager.PublishMessage(publishCommand);
                break;
            default:
                throw new NotImplementedException();
        }
    }

    public async Task SendResponce(IResponceCommand responce, ITcpConnection connection)
    {
        await connection.SendMessageAsync(responce.ToBuffer());
    }
}