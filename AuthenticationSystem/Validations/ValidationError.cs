namespace AuthenticationSystem.Validations
{
    public class ValidationError
    {
        public string PropertyName { get; init; }

        public string[] Errors { get; init; }
    }
}
