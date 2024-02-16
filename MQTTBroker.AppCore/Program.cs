using MQTTBroker.AppCore.Services;

namespace MQTTBroker.AppCore;

public class Program
{
    public static void Main(string[] args)
    {
        var broker = Broker.GetBroker();
        broker.Start();
        Console.WriteLine("Press any key to stop the broker");
        Console.ReadKey();
        broker.Stop();
    }
}