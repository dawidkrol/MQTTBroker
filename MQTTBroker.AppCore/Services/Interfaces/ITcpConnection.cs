namespace MQTTBroker.AppCore.Services.Interfaces
{
    public interface ITcpConnection
    {
        void SetConnectionEstablished(bool value);
        bool IsConnectionEstablished();
        void SetClientId(string clientId);
        string GetClientId();
        void Close();
        void Dispose();
        bool IsConnected();
        Task SendMessageAsync(byte[] message);
        Task StartAsync();
    }
}