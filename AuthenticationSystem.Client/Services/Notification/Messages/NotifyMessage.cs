namespace AuthenticationSystem.Client.Services.Notification.Messages
{
    public class NotifyMessage
    {

        public string Message { get; }
        public MessageType MessageType { get; }
        public string Icon { get; }

        public NotifyMessage(string message, MessageType messageType)
        {
            Message = message;
            MessageType = messageType;
            Icon = ResolveIcon(messageType);
        }

        public static NotifyMessage SuccessMessage(string message)
        {
            return new NotifyMessage(message, MessageType.Success);
        }

        public static NotifyMessage ErrorMessage(string message)
        {
            return new NotifyMessage(message, MessageType.Error);
        }

        public static NotifyMessage WarningMessage(string message)
        {
            return new NotifyMessage(message, MessageType.Warning);
        }

        public static NotifyMessage InfoMessage(string message)
        {
            return new NotifyMessage(message, MessageType.Info);
        }

        private static string ResolveIcon(MessageType type)
        {
            return type switch
            {
                MessageType.Success => NotifyDefaults.SuccessIcon,
                MessageType.Error => NotifyDefaults.ErrorIcon,
                MessageType.Warning => NotifyDefaults.WarningIcon,
                MessageType.Info => NotifyDefaults.InfoIcon,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
    }

    public static class NotifyDefaults
    {
        public const string SuccessIcon = "success";
        public const string ErrorIcon = "error";
        public const string WarningIcon = "warning";
        public const string InfoIcon = "info";
    }
}
