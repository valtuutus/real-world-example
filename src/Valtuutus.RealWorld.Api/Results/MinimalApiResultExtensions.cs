using System.Net;

namespace Valtuutus.RealWorld.Api.Results;

public static partial class ResultExtensions
{
    public static Microsoft.AspNetCore.Http.IResult ToApiResult<T>(this Result<T> result)
    {
        return result.Match(
            success => success.ToApiResult(),
            error => error.ToApiResult()
        );
    }

    public static Microsoft.AspNetCore.Http.IResult ToApiResult(this ErrorResult result)
    {
        return result.Type switch
        {
            ErrorType.Error => TypedResults.StatusCode((int)HttpStatusCode.InternalServerError),
            ErrorType.Forbidden => TypedResults.Forbid(),
            ErrorType.Unauthorized => TypedResults.Unauthorized(),
            ErrorType.Invalid => TypedResults.BadRequest(result.Errors),
            ErrorType.NotFound => TypedResults.NotFound(),
            ErrorType.Conflict => TypedResults.Conflict(),
            _ => throw new InvalidOperationException(),
        };
    }

    public static Microsoft.AspNetCore.Http.IResult ToApiResult<T>(this SuccessResult<T> result)
    {
        return TypedResults.Ok(result.Value);
    }
}