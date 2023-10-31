using FluentResults;

namespace AuthenticationSystem.Domain.Extensions
{
    internal static class ResultExtensions
    {
        internal static Result<T> Ok<T>(this T value)
        {
            return Result.Ok(value);
        }

        internal async static Task<Result<T>> Ok<T>(this Task<T> task)
        {
            return Result.Ok(await task);
        }

        internal async static Task<Result> Fail(this Task<string> task)
        {
            return Result.Fail(await task);
        }

        internal static Result ToResult(this Exception ex)
        {
            return Result.Fail(new ExceptionalError(ex));
        }

        internal static Result Fail(this string errorMessage)
        {
            return Result.Fail(errorMessage);
        }
    }
}
