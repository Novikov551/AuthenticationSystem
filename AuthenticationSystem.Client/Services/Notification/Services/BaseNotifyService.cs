using AuthenticationSystem.Client.Services.Notification.Messages;
using Microsoft.AspNetCore.SignalR;

namespace AuthenticationSystem.Client.Services.Notification.Services
{
    public abstract class BaseNotifyService
    {
        protected readonly IHubContext<AuthHub> HubContext;
        protected readonly FrontendCommunicationService UserConnections;
        private readonly ILogger<BaseNotifyService> _logger;

        protected BaseNotifyService(IHubContext<AuthHub> hubContext, FrontendCommunicationService userConnections,
            ILogger<BaseNotifyService> logger)
        {
            HubContext = hubContext;
            UserConnections = userConnections;
            _logger = logger;
        }

        protected virtual async Task SendCloseModalAsync(IEnumerable<string> connectionIds, string modalDiv)
        {
            await HubContext.Clients.Clients(connectionIds).SendAsync("CloseModal", new CloseModalMessage(modalDiv));
        }

        protected virtual async Task SendSuccessNotifyAsync(IEnumerable<string> connectionIds, string message)
        {
            try
            {
                await HubContext.Clients.Clients(connectionIds).SendAsync("Notify", NotifyMessage.SuccessMessage(message));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Произошла ошибка при отправке оповещения");
            }
        }

        protected virtual async Task SendWarningNotifyAsync(IEnumerable<string> connectionIds, string message)
        {
            try
            {
                await HubContext.Clients.Clients(connectionIds).SendAsync("Notify", NotifyMessage.WarningMessage(message));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Произошла ошибка при отправке оповещения");
            }
        }

        protected virtual async Task SendErrorNotifyAsync(IEnumerable<string> connectionIds, string message)
        {
            try
            {
                await HubContext.Clients.Clients(connectionIds).SendAsync("Notify", NotifyMessage.ErrorMessage(message));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Произошла ошибка при отправке оповещения");
            }

        }

        protected virtual async Task SendInfoNotifyAsync(IEnumerable<string> connectionIds, string message)
        {
            try
            {
                await HubContext.Clients.Clients(connectionIds).SendAsync("Notify", NotifyMessage.InfoMessage(message));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Произошла ошибка при отправке оповещения");
            }
        }

        protected virtual async Task ReloadTableAsync(IEnumerable<string> connectionIds, string functionName)
        {
            await HubContext.Clients.Clients(connectionIds).SendAsync("ReloadTable", new ReloadTableMessage(functionName));
        }

        protected virtual async Task ReloadTableWithArgumentAsync(IEnumerable<string> connectionIds, string functionName, object argument)
        {
            await HubContext.Clients.Clients(connectionIds).SendAsync("ReloadTableWithArgument", new ReloadTableWithArgument(functionName, argument));
        }

        protected virtual async Task ReloadTableWithArgumentsAsync(IEnumerable<string> connectionIds, string functionName, object first, object second)
        {
            await HubContext.Clients.Clients(connectionIds).SendAsync("ReloadTableWithArguments", new ReloadTableWithArguments(functionName, first, second));
        }
    }
}
