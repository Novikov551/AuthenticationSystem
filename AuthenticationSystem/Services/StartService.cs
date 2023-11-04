namespace AuthenticationSystem.Services
{

    public class StartService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public StartService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _scopeFactory.CreateScope();
        }
    }
}
