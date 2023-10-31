using AuthenticationSystem.Client.Config;
using AuthenticationSystem.Shared;
using FluentResults;

using Microsoft.Extensions.Options;
using AuthenticationSystem.Domain.Extensions;

namespace AuthenticationSystem.Client.Services.Rest;

public class AuthApiService : IAuthApiService
{
    private readonly HttpClient _httpClient;
    private readonly AuthApiConfig _config;
    private readonly ILogger<AuthApiService> _logger;

    public AuthApiService(HttpClient httpClient,
        IOptions<AuthApiConfig> config, ILogger<AuthApiService> logger)
    {
        _config = config.Value;
        _httpClient = httpClient;
        _logger = logger;
        _httpClient.BaseAddress = new Uri(config.Value.BaseAdress);
    }

    public async Task<Result<UserResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(_config.Login, request, cancellationToken: cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<UserResponse>(cancellationToken: cancellationToken).Ok();
            }

            return await response.Content.ReadAsStringAsync(cancellationToken).Fail();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "При запросе API `{Method}` произошла ошибка", nameof(LoginAsync));
            return ApiErrorCodes.HttpTimeoutError.Fail();
        }
    }

    public async Task<Result<UserResponse>> RegisterAsync(CreateUserRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(_config.Register, request, cancellationToken: cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<UserResponse>(cancellationToken: cancellationToken).Ok();
            }

            return await response.Content.ReadAsStringAsync(cancellationToken).Fail();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "При запросе API `{Method}` произошла ошибка", nameof(RegisterAsync));
            return ApiErrorCodes.HttpTimeoutError.Fail();
        }
    }
}
