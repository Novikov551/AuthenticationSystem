using AuthenticationSystem.Shared;
using FluentResults;
using UserAuthenticationSystem.Domain.Core;

namespace AuthenticationSystem.Logic.Users
{
    public interface IUserManager<T, TCreateModel>
    {
        public Task<T> FindByEmailAsync(string email);
        public Task<T> FindByIdAsync(Guid Id);
        public Task<IEnumerable<T>> FindByFullNameAsync(string name, string surname);
        public Task<T> ChangePassword(string email, string newPassowrd);
        public Task DeleteUser(Guid userId);
        public Task<Result<T>> CreateUser(TCreateModel user);
        public Task<Result<T>> LoginAsync(string email, string password);
    }
}
