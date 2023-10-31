namespace AuthenticationSystem.Logic.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public string Key { get; set; }
        public string PropertyName { get; set; }
        public Type EntityType { get; set; }

        public EntityNotFoundException(Type entityType, Guid key)
        {
            EntityType = entityType;
            Key = key.ToString();
        }

        public EntityNotFoundException(Type entityType, string key)
        {
            EntityType = entityType;
            Key = key;
        }

        public EntityNotFoundException(Type entityType, string key, string propertyName) : this(entityType, key)
        {
            PropertyName = propertyName;
        }
    }
}
