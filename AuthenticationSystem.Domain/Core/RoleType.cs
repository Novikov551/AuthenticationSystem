namespace AuthenticationSystem.Domain.Core
{
    [Flags]
    public enum RoleType : byte
    {
        None = 0,
        User = 00000001 ,
        Admin = 00000010 ,
        Moderator = 00000100
    }
}
