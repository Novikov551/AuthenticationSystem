using AuthenticationSystem.Client.Services.Notification.Services;
using Microsoft.AspNetCore.SignalR;

namespace AuthenticationSystem.Client.Services.Notification.Messages
{
    public class NotifyService : BaseNotifyService
    {
        public NotifyService(IHubContext<AuthHub> hubContext,
            FrontendCommunicationService communicationService, ILogger<NotifyService> logger)
            : base(hubContext, communicationService, logger)
        {
        }
    }
}
