using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using AuthenticationSystem.Validations;

namespace AuthenticationSystem.Filters
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ErrorHandlingAttribute : TypeFilterAttribute
    {
        public ErrorHandlingAttribute()
            : base(typeof(ErrorHandlingFilterAttributeImpl))
        {

        }

        private static readonly IEnumerable<Type> _exceptions = new[] { typeof(SystemException) };

        private sealed class ErrorHandlingFilterAttributeImpl : ExceptionFilterAttribute
        {
            private readonly ILogger<ErrorHandlingAttribute> _logger;

            public ErrorHandlingFilterAttributeImpl(ILogger<ErrorHandlingAttribute> logger)
            {
                _logger = logger;
            }

            public override void OnException(ExceptionContext context)
            {
                var exception = context.Exception;

                var occurredExceptionType = context.Exception.GetType();

                if (_exceptions.Any(exceptionType =>
                    occurredExceptionType == exceptionType || occurredExceptionType.IsSubclassOf(exceptionType)))
                {
                    context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                    context.Result = new JsonResult(HandleException(exception));
                }
                else if (exception is ValidationException validationException)
                {
                    context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                    context.Result = HandleValidationException(validationException);
                }
                else
                {
                    context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Result = new JsonResult(HandleException(exception));
                }

                _logger.LogError(exception, "Произошла ошибка во время обработки запроса");

                base.OnException(context);
            }

            private static string HandleException(Exception exception)
            {
                return exception.InnerException != default
                    ? $"{exception.Message} --> InnerException : {HandleException(exception.InnerException)}"
                    : exception.Message;
            }

            private static JsonResult HandleValidationException(ValidationException exception)
            {
                return new JsonResult(exception.CreateResponse());
            }
        }
    }
}
