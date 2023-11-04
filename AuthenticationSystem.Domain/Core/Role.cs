namespace AuthenticationSystem.Domain.Core
{
    public class Role : BaseEntity
    {
        public string RoleName { get; set; }
        public RoleType RoleType { get; set; }
    }
}
