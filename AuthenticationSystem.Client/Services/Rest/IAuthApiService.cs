using AuthenticationSystem.Shared;
using FluentResults;

namespace AuthenticationSystem.Client.Services.Rest;

public interface IAuthApiService
{
    public Task<Result<UserResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
    public Task<Result<UserResponse>> RegisterAsync(CreateUserRequest request, CancellationToken cancellationToken = default);
}
