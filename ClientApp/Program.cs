using System.Text;
using MQTTnet;
using MQTTnet.Client;

namespace MqttPublisher;

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

        await Task.Delay(2000);

        var result = await client.SubscribeAsync(mqttSubscribeOptions);

        await Task.Delay(2000);

        await SendMessages(client);
        //await client.PingAsync();

        await Task.Delay(2000);

        await client.UnsubscribeAsync("DUPA");

        await Task.Delay(2000);


        //await client.SubscribeAsync(mqttSubscribeOptions);

        //SendMessages(client);

        //await client.UnsubscribeAsync("DUPA/Disconnect");

        await CleanDisconnect(client);

        Console.ReadLine();
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
        // Use builder classes where possible in this project.

        //string certFilePath = "../Certificates/http.pfx";
        //string password = "RVbySf#FV8*!xG4&o4j6";

        //var certs = new List<X509Certificate2>
        //{
        //    new (certFilePath)
        //};

        var mqttClientOptions = new MqttClientOptionsBuilder()
            .WithClientId($"publ")
            .WithProtocolVersion(MQTTnet.Formatter.MqttProtocolVersion.V310)
            .WithWillQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtMostOnce)
            .WithTcpServer("localhost", 1884)
            .Build();


        // This will throw an exception if the server is not available.
        // The result from this message returns additional data which was sent
        // from the server. Please refer to the MQTT protocol specification for details.
        var response = await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);
        //
        Console.WriteLine("The MQTT client is connected.");

        Console.Write(response.ResponseInformation);

        return mqttClient;
    }

    private static async Task SendMessages(IMqttClient mqttClient)
    {
        //do
        //{
        //Console.WriteLine("Write your message!");
        var message = "message";

        //Console.WriteLine("Now name topic:");
        var topic = "DUPA";

        var messageBytes = Encoding.UTF8.GetBytes(message!);
        var data = await mqttClient.PublishBinaryAsync(topic, messageBytes);
        await Console.Out.WriteLineAsync(data.IsSuccess.ToString());

        //Console.WriteLine("Message sent. Continue or press Q to quit");
        //    //wait 5 sec
        //    Task.Delay(TimeSpan.FromSeconds(5)).Wait();
        //} while (Console.ReadKey().Key != ConsoleKey.Q);
    }
}