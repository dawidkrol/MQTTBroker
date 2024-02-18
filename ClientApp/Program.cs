using System.Text;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;

namespace ClientApp;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var client = await ConnectClient();

        client.ApplicationMessageReceivedAsync += delegate(MqttApplicationMessageReceivedEventArgs args)
        {
            Console.WriteLine(Encoding.Default.GetString(args.ApplicationMessage.PayloadSegment));
            return Task.CompletedTask;
        };

        var mqttSubscribeOptions = new MqttFactory().CreateSubscribeOptionsBuilder()
            .WithTopicFilter(
                f =>
                {
                    f.WithTopic("DUPA");
                    f.WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtMostOnce);
                    f.WithRetainAsPublished(false);
                }
            )
            .Build();

        await client.SubscribeAsync(mqttSubscribeOptions);
        
        await SendMessages(client);

        await client.UnsubscribeAsync("DUPA");
        await SendMessages(client);
        await client.SubscribeAsync(mqttSubscribeOptions);
        await client.UnsubscribeAsync("DUPA");
        
        Console.ReadLine();
        await CleanDisconnect(client);
    }

    private static async Task CleanDisconnect(IMqttClient mqttClient)
    {
        await mqttClient.DisconnectAsync(new MqttClientDisconnectOptionsBuilder()
            .WithReason(MqttClientDisconnectOptionsReason.NormalDisconnection).Build());
    }

    private static async Task<IMqttClient> ConnectClient()
    {
        var mqttFactory = new MqttFactory();

        var mqttClient = mqttFactory.CreateMqttClient();

        var mqttClientOptions = new MqttClientOptionsBuilder()
            .WithClientId($"publ")
            .WithProtocolVersion(MQTTnet.Formatter.MqttProtocolVersion.V310)
            .WithWillQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtMostOnce)
            .WithTcpServer("localhost", 1884)
            .Build();
        
        var response = await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);
        Console.WriteLine("The MQTT client is connected.");

        Console.Write(response.ResponseInformation);

        return mqttClient;
    }

    private static async Task SendMessages(IMqttClient mqttClient)
    {
        var message = "message";
        var topic = "DUPA";

        var messageBytes = Encoding.UTF8.GetBytes(message);
        var data = await mqttClient.PublishBinaryAsync(topic, messageBytes, MqttQualityOfServiceLevel.AtMostOnce);
        await Console.Out.WriteLineAsync(data.IsSuccess.ToString());
    }
}