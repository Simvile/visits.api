namespace visits.api.Utils;

public class ResponseHandler
{
    public Guid Id { get; set; }
    public List<ResponseMessage> Messages { get; set; } = [];

    public bool HasErrorMessage =>
        Messages.Any(m => m.Type == ResponseType.ErrorMessage);

    public bool HasWarningMessage =>
        Messages.Any(m => m.Type == ResponseType.WarningMessage);

    public void AddMessage(string message)
        => Messages.Add(new ResponseMessage
        {
            Type =  ResponseType.ErrorMessage,
            Text = message
        });
    
    public void AddMessage(string message, ResponseType messageType)
        => Messages.Add(new ResponseMessage
        {
            Type =  messageType,
            Text = message
        });
    
}

public enum ResponseType
{
    SuccessMessage = 1,
    ErrorMessage = 2,
    WarningMessage = 3
}

public class ResponseMessage
{
    public ResponseType Type { get; set; }
    public string Text { get; set; }
}