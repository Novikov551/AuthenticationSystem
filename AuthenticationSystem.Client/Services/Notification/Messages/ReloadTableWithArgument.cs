namespace AuthenticationSystem.Client.Services.Notification.Messages
{
    public class ReloadTableWithArgument
    {
        public object Argument { get; }

        public string FunctionName { get; }

        public ReloadTableWithArgument()
        {

        }

        public ReloadTableWithArgument(string functionName, object argument)
        {
            FunctionName = functionName;
            Argument = argument;
        }
    }
}
