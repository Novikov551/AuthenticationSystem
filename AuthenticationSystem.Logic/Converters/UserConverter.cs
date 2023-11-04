using AuthenticationSystem.Domain.Core;
using AuthenticationSystem.Shared;
using UserAuthenticationSystem.Domain.Core;

namespace AuthenticationSystem.Logic.Converters
{
    public static class UserConverter
    {
        public static UserResponse Convert(User user, Role role)
        {
            RoleResponse userRole = null;
            if (role is not null)
            {
                userRole = new RoleResponse
                {
                    RoleId = role.Id,
                    RoleType = role.RoleType
                };
            }

            return new UserResponse
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                BirthDate = user.BirthDate,
                PhoneNumber = user.PhoneNumber,
                Role = userRole
            };
        }
    }
}
