namespace AuthenticationSystem.Client.Services.Notification.Messages
{
    public class CloseModalMessage
    {
        public string ModalDiv { get; }

        public CloseModalMessage(string modalDiv)
        {
            ModalDiv = modalDiv;
        }
    }
}
