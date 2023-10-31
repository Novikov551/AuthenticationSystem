namespace AuthenticationSystem.Domain.Core
{
    public class BaseEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Guid Version { get; set; }
    }
}
