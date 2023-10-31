using FluentValidation;
using FluentValidation.Results;

namespace AuthenticationSystem.Validations
{
    public static class ValidationExceptionExtensions
    {
        public static ValidationErrorResponse CreateResponse(this ValidationException ex)
        {
            ILookup<string, string> source = ex.Errors.ToLookup((ValidationFailure k) => k.PropertyName, (ValidationFailure v) => v.ErrorMessage);
            return new ValidationErrorResponse
            {
                Errors = source.Select((IGrouping<string, string> e) => new ValidationError
                {
                    PropertyName = e.Key,
                    Errors = e.Select((string message) => message).ToArray()
                }).ToArray()
            };
        }
    }
}
