using AuthenticationSystem.Domain.Core;

namespace UserAuthenticationSystem.Domain.Core
{
    public class User : BaseEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string EmailHash { get; set; }
        public string PasswordHash { get; set; }
    }
}
