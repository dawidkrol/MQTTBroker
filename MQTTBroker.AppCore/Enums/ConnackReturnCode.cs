namespace MQTTBroker.AppCore.Enums;

public enum ConnackReturnCode
{
    ConnectionAccepted = 0,
    UnacceptableProtocolVersion = 1,
    IdentifierRejected = 2,
    ServerUnavailable = 3,
    BadUsernameOrPassword = 4,
    NotAuthorized = 5
}
