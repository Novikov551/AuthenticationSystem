namespace AuthenticationSystem.Client.Services.Notification.Messages
{
    public class ReloadTableWithArguments
    {
        public object First { get; }
        public object Second { get; }
        public string FunctionName { get; }

        public ReloadTableWithArguments()
        {

        }

        public ReloadTableWithArguments(string functionName, object first, object second)
        {
            FunctionName = functionName;
            First = first;
            Second = second;
        }
    }
}
