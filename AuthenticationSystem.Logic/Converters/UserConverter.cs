using AuthenticationSystem.Shared;
using UserAuthenticationSystem.Domain.Core;

namespace AuthenticationSystem.Logic.Converters
{
    public static class UserConverter
    {
        public static UserResponse Convert(User user)
        {
            return new UserResponse
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
            };
        }
    }
}
