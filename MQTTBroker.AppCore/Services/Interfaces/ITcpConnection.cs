namespace MQTTBroker.AppCore.Services.Interfaces
{
    public interface ITcpConnection
    {
        bool IsConnectionEstablished { get; set; }
        string ClientId { get; set; }
        void Close();
        void Dispose();
        bool IsConnected();
        Task SendMessageAsync(byte[] message);
        Task StartAsync();
    }
}