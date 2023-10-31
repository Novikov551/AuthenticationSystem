using Microsoft.AspNetCore.SignalR;

namespace AuthenticationSystem.Client.Services.Notification.Services
{
    public class AuthHub : Hub
    {
        private readonly ILogger<AuthHub> _logger;
        private readonly FrontendCommunicationService _frontendCommunicationService;

        public AuthHub(ILogger<AuthHub> logger, FrontendCommunicationService frontendCommunicationService)
        {
            _logger = logger;
            _frontendCommunicationService = frontendCommunicationService;
        }


        //public override async Task OnConnectedAsync()
        //{
        //    _logger.LogDebug("Connected {User}", Context.User.Identity.Name);
        //    _frontendCommunicationService.AddUserConnectionId(Context.User.Identity.GetSubjectId(), Context.ConnectionId);
        //    await base.OnConnectedAsync();
        //}

        //public override Task OnDisconnectedAsync(Exception exception)
        //{
        //    _frontendCommunicationService.DeleteUserConnectionId(Context.User.Identity.GetSubjectId(), Context.ConnectionId);
        //    _logger.LogDebug("Disconnected");
        //    return Task.CompletedTask;
        //}
    }
}
