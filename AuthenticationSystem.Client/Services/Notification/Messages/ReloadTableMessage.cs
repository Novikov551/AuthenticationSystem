namespace AuthenticationSystem.Client.Services.Notification.Messages
{
    public class ReloadTableMessage
    {
        public string FunctionName { get; }
        public ReloadTableMessage(string functionName)
        {
            FunctionName = functionName;
        }
    }
}
