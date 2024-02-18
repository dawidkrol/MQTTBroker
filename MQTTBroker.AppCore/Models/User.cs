namespace MQTTBroker.AppCore.Models;

public class User
{
    public string Username { get; set; }
    public string Password { get; set; }

    public bool CheckCredentials(string username, string password)
    {
        return Username == username && Password == password;
    }
}