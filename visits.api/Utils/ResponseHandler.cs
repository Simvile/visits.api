namespace visits.api.Utils;

public class ResponseHandler
{
    public Guid Id { get; set; }
    public ResponseType Type { get; set; }
    public List<string> Messages { get; set; } = [];

    public void AddMessage(string message)
    {
        Messages.Add(message);
    }
    
    public void AddMessage(string message, ResponseType messageType)
    {
        Type = messageType;
        Messages.Add(message);
    }
}

public enum ResponseType
{
    SuccessMessage,
    ErrorMessage,
    WarningMessage
}