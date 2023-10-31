using System.Security.Claims;

namespace AuthenticationSystem.Client.Services.Rest
{
    public class BackendApiAuthenticationHttpClientHandler : DelegatingHandler
    {
        private readonly ClaimsPrincipal _claimsPrincipal;

        public BackendApiAuthenticationHttpClientHandler(ClaimsPrincipal claimsPrincipal)
        {
            _claimsPrincipal = claimsPrincipal;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //request.Headers.Authorization = new AuthenticationHeaderValue(CookieAuthenticationDefaults.AuthenticationScheme);
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
