using System.Collections.Concurrent;
using MQTTBroker.AppCore.Commands;
using MQTTBroker.AppCore.Commands.RequestCommands;
using MQTTBroker.AppCore.Services.Interfaces;

namespace MQTTBroker.AppCore.Services;

public class Broker : IBroker
{
    private static Broker _instance;
    private const string Host = "127.0.0.1";
    private const int Port = 1884;
    private readonly IConnectionListener _connectionListener;
    private readonly ITopicManager _topicManager;
    private readonly IConnectionManager _connectionManager;
    private readonly ConcurrentQueue<ICommand> _commands = new();
    private CancellationTokenSource _cts = new();
    private Task _commandExecutionTask;

    public static IBroker GetBroker()
    {
        _instance ??= new Broker();
        return _instance;
    }

    private Broker()
    {
        _connectionListener = new ConnectionListener(Host, Port, this);
        _connectionManager = new ConnectionManager(this);
        _topicManager = new TopicManager(this);
        _commandExecutionTask = Task.Run(() => ExecuteCommands(_cts.Token));
    }

    public void Start()
    {
        _connectionListener.StartListening();
    }

    public void AddCommandToQueue(ICommand command)
    {
        _commands.Enqueue(command);
    }

    private async Task ExecuteCommands(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            if (_commands.TryDequeue(out ICommand command))
            {
                await ExecuteCommand(command);
            }
            else
            {
                await Task.Delay(100, cancellationToken); // wait for a while before checking the queue again
            }
        }
    }

    public async void Stop()
    {
        _connectionListener.StopListening();
        await _commandExecutionTask.WaitAsync(_cts.Token);
        await _cts.CancelAsync();
    }

    private async Task ExecuteCommand(ICommand command)
    {
        Console.WriteLine("Executing command: " + command.GetType().Name);
        switch (command)
        {
            case CreateTcpConnectionCommand createTcpConnectionCommand:
                await _connectionManager.AddConnection(createTcpConnectionCommand);
                break;
            case ConnectCommand connectCommand:
                _connectionManager.AddTcpConnection(connectCommand.Client);
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
            case PingReqCommand pingReqCommand:
                await _connectionManager.Ping(pingReqCommand);
                break;
            default:
                throw new NotImplementedException();
        }
    }

    public async Task SendResponse(IResponseCommand response, ITcpConnection connection)
    {
        await connection.SendMessageAsync(response.ToBuffer());
    }
}