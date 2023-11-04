using AuthenticationSystem.Domain.Core;

namespace AuthenticationSystem.Client.Models
{
    public class AccessDeniedViewModel
    {
        public RoleType? RoleType { get; set; }

        public AccessDeniedViewModel(string role)
        {
            RoleType = (RoleType)byte.Parse(role);
        }

        public bool ShowRoleTypeId => RoleType.HasValue;
    }
}
